﻿using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Kosmos2;
using MLAgents;
using OpenAIGPT;
using TensorData;
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
        [SerializeField] private string behaviourName;
        [SerializeField] private string resultsPath = "/mnt/d/UserProjects/Joey/Unity/UnityLLM/results";
        [SerializeField] private string mlAgentsConfigPath = "/mnt/d/UserProjects/Joey/Unity/ML_Demos/Config/ballance_plate_config.yaml";
        [SerializeField] private List<GPTConversationPoint> OnResponseActions;
        [SerializeField] private TensorDataConnector tensorDataConnector;

        private string summary;
        private int counter;
        private HyperParameterConfig hyperConfig;

        private string FEventsPath => $"{resultsPath}/{behaviourName}/{behaviourName}/";

        private void Start()
        {
            LoggingController.Log($"--- NEW EPISODE -- ");

            GPTResponse("");//Empty to kick off the flow
            gptConverser.OnResponse.AddListener(GPTResponse);
            observer.OnResponse.AddListener(ObserverResponse);
            observer.OnGroundedResponse.AddListener(GroundedObservationResponse);
        }

        private void GPTResponse(string response)
        {
            ProcessNextPoint(response);
        }

        private void ProcessNextPoint(string response = "")
        {
            var point = OnResponseActions[counter];
            counter++; //has to happen right after the reference to avoid recursive calls
            Debug.Log($"Current Response Action: {point.name}");
            Debug.Log($"GPT Response: {response}");
            
            LoggingController.Log($"[MODERATOR] Current Response Action: {point.name}");

            if (point.sendsContext){
                point.action.Invoke(point.context);

            }else if (point.sendsResponse){
                point.action.Invoke(response);
            }

            if (point.addToSummary) summary += response;

            if (point.altAction != null)
            {
                point.altAction.Invoke();
            }
        }
       
        public void StartFormatSummary()
        {
            LLMRLMetaController.currentSummary = summary;//TODO should be moved
            StartCoroutine(CollectSummary());
        }
        
        //I'm creating a "promise" training summary here - never assume every aspect of the summary is filled in
        private IEnumerator CollectSummary()
        {
            var trainingSummary = new TrainingSummary();
            trainingSummary.contextSummary = summary;
            
            var previousSessionData = MetaSessionDataController.RetrieveSessionData(LLMRLMetaController.Instance.sessionPath);
            
            if (previousSessionData?.codeHistory.Count > 0) // has code history
            {
                trainingSummary.previousCode = previousSessionData.codeHistory.Last();
            }

            yield return tensorDataConnector.RequestTensorData(FEventsPath, (prevData) =>
            {
                trainingSummary.previousTensorData = prevData;
            });
            
            Debug.Log(trainingSummary);
            
            //At end of training - read hyper parameters from config file
            //hyperConfig = HyperParameterBridge.Read(mlAgentsConfigPath);//Read hyper parameters from config file
            //Share observation and ask new hyperparameters from GPT (hyperParameterRequest)
            //Store new hyperparameters in config file HyperParameterBridge.Write(object, mlAgentsConfigPath);
            
            gptConverser.PrepareSummary(trainingSummary);
            
            ProcessNextPoint();
        }

        public void StartCode(string response)
        {
            cliBridge.RunMLAgentsInWSL(behaviourName, mlAgentsConfigPath); //Start it before compiling and running the code, as it might take a while to spin up the process
            StartCoroutine(CompileDelay(response)); //Wait a bit for the ml-agents to start up
        }

        private IEnumerator CompileDelay(string response)
        {
            yield return new WaitForSeconds(5.0f);
            codeBridge.Process(behaviourName, response);
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
            summary += relationDescription.ToString();
        }
        
    }
}