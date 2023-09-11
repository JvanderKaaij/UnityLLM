﻿using System.Collections.Generic;
using System.Text;
using Kosmos2;
using MLAgents;
using OpenAIGPT;
using Unity.MLAgents;
using UnityEngine;

namespace DefaultNamespace
{
    public class ConversationModerator:MonoBehaviour
    {
        [SerializeField] LLMOObserver observer;
        [SerializeField] GPTConverser gptConverser;
        [SerializeField] GPTCSharpBridge codeBridge;
        [SerializeField] CLIBridge cliBridge;
        [SerializeField] private string configPath = "/mnt/d/UserProjects/Joey/Unity/ML_Demos/Config/ballance_plate_config.yaml";
        
        
        private int counter;
        private HyperParameterConfig hyperConfig;

        [TextAreaAttribute]
        [SerializeField] private string preContext;
        
        [TextAreaAttribute]
        [SerializeField] private string afterQueries;
        
        [TextAreaAttribute]
        [SerializeField] private string addedContext;
        
        [TextAreaAttribute]
        [SerializeField] private string furtherContext;
        
        [TextAreaAttribute (10, 10)]
        [SerializeField] private string codeRequest;
        
        [TextAreaAttribute]
        [SerializeField] private string codeReconsider;
        
        [TextAreaAttribute]
        [SerializeField] private string hyperParameterRequest;
        
        private void Start()
        {
            gptConverser.CallMessage(preContext);
            gptConverser.OnResponse.AddListener(GPTResponse);
            observer.OnResponse.AddListener(ObserverResponse);
            observer.OnGroundedResponse.AddListener(GroundedObservationResponse);
        }

        private void GPTResponse(string response)
        {
            counter++;
            if(counter <= 5){
                observer.Observe(response);
            }else if (counter == 6) {   
                gptConverser.CallMessage(afterQueries);
            }else if (counter == 7){
                observer.ObserveGrounded();
            }else if (counter == 8) {
                gptConverser.CallMessage(addedContext);
            }else if (counter == 9) {
                gptConverser.CallMessage(furtherContext);
            }else if (counter == 10) { 
                gptConverser.CallMessage(codeRequest);
            }else if (counter == 11){
                gptConverser.CallMessage(codeReconsider);
                cliBridge.RunMLAgentsInWSL(configPath);//Start it before compiling and running the code, as it might take a while to spin up the process
                
                //TODO Add HyperParameter Flow:
                //At end of training - read hyper parameters from config file
                //hyperConfig = HyperParameterBridge.Read(configPath);//Read hyper parameters from config file
                //Share observation and ask new hyperparameters from GPT (hyperParameterRequest)
                //Store new hyperparameters in config file HyperParameterBridge.Read(object, configPath);
                
            }else if (counter == 12)
            {
                codeBridge.Process(response);
            }
        }

        private void ObserverResponse(string response)
        {
            string last = gptConverser.GetLastMessage();
            //remote last string from response
            response = response.Replace(last, "");
            response = response.Replace(last, "");
            gptConverser.CallMessage(response);
        }

        private void GroundedObservationResponse(List<Kosmos2GroundedRelation> groundedRelations)
        {
            StringBuilder relationDescription = new StringBuilder();
            foreach (var relation in groundedRelations)
            {
                relationDescription.Append(
                    $"{relation.label} could be find by the name {relation.subject.name} in the Unity Hierarchy. It contains the following Components: {relation.components}");
            }
            gptConverser.CallMessage(relationDescription.ToString());
        }
        
    }
}