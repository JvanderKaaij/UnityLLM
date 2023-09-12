﻿using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private List<GPTConversationPoint> OnResponseActions;
        
        private int counter;
        private HyperParameterConfig hyperConfig;

        
        private void Start()
        {
            GPTResponse("");//Empty to kick off the flow
            gptConverser.OnResponse.AddListener(GPTResponse);
            observer.OnResponse.AddListener(ObserverResponse);
            observer.OnGroundedResponse.AddListener(GroundedObservationResponse);
        }

        private void GPTResponse(string response)
        {
            Debug.Log($"Response Counter: {counter}");
            Debug.Log($"GPT Response: {response}");
            
            var point = OnResponseActions[counter];
            
            if (point.sendsContext){
                point.action.Invoke(point.context);

            }else if (point.sendsResponse){
                point.action.Invoke(response);
            }

            if (point.altAction != null)
            {
                point.altAction.Invoke();
            }

            counter++;
        }

        public void StartCode(string response)
        {
            cliBridge.RunMLAgentsInWSL(configPath); //Start it before compiling and running the code, as it might take a while to spin up the process
            StartCoroutine(CompileDelay(response)); //Wait a bit for the ml-agents to start up
        }

        private IEnumerator CompileDelay(string response)
        {
            yield return new WaitForSeconds(5.0f);
            codeBridge.Process(response);
        }

        private void ObserverResponse(string response)
        {
            string last = gptConverser.GetLastMessage();
            //remote last string from response
            response = response.Replace(last, "");
            
            Debug.Log($"Kosmos Replied to GPT: {response}");
            
            gptConverser.Prompt(response);
        }

        private void GroundedObservationResponse(List<Kosmos2GroundedRelation> groundedRelations)
        {
            StringBuilder relationDescription = new StringBuilder();
            foreach (var relation in groundedRelations)
            {
                relationDescription.Append(
                    $"{relation.label} could be find by the name {relation.subject.name} in the Unity Hierarchy. It contains the following Components: {relation.components}");
            }
            gptConverser.Prompt(relationDescription.ToString());
        }
        
    }
}