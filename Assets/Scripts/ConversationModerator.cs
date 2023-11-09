using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Kosmos2;
using MLAgents;
using OpenAIGPT;
using TensorData;
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
        [SerializeField] private string hyperParamPath = "D:/UserProjects/Joey/Unity/ML_Demos/Config/ballance_plate_config.yaml";
        [SerializeField] private List<GPTConversationPoint> OnResponseActions;
        [SerializeField] private TensorDataConnector tensorDataConnector;

        TrainingSummary trainingSummary;
        private string summary;
        private int counter;
        private string codeResponse;

        public static bool RUNNING;
        
        private string FEventsPath => $"{resultsPath}/{behaviourName}/{behaviourName}/";

        private void Start()
        {
            LoggingController.Log($"--- NEW EPISODE -- ");

            // GPTResponse("");//Empty to kick off the flow
            gptConverser.OnResponse.AddListener(GPTResponse);
            //TODO: Disabled for refactor to GPT Image
            //observer.OnResponse.AddListener(ObserverResponse);
            //observer.OnGroundedResponse.AddListener(GroundedObservationResponse);
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
            
            LoggingController.Log($" - Process Next Point - [MODERATOR] Current Response Action: {point.name}");

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
            trainingSummary.contextSummary = summary;
            
            var previousSessionData = MetaSessionDataController.RetrieveSessionData(LLMRLMetaController.Instance.sessionPath);
            
            if (previousSessionData?.codeHistory.Count > 0) // has code history
            {
                trainingSummary.previousCode = previousSessionData.codeHistory.Last();
            }
            
            if (previousSessionData?.errorHistory.Count > 0) // has code error history
            {
                trainingSummary.previousCodeError = previousSessionData.errorHistory.Last();
            }

            yield return tensorDataConnector.RequestTensorData(FEventsPath, (prevData) =>
            {
                trainingSummary.previousTensorData = prevData;
            });


            trainingSummary.previousHyperParams = HyperParameterBridge.Read(hyperParamPath);

            Debug.Log(trainingSummary);
            
            gptConverser.PrepareSummary(trainingSummary);
            ProcessNextPoint();
        }
        public void ApplyHyperParameters(string response)
        {
            Debug.Log("Suggested Hyper Parameters:");
            Debug.Log(response);
            Match match = Regex.Match(response, @"```json\s*(.*?)```", RegexOptions.Singleline);
            
            if (match.Success)
            {
                var newHyperParameters = match.Groups[1].Value.Trim();
                Debug.Log("FOUND HYPER PARAMS:");
                Debug.Log(newHyperParameters);
                Hyperparameters hyperParameter = JsonUtility.FromJson<Hyperparameters>(newHyperParameters);
                 var newParams = trainingSummary.previousHyperParams;
                newParams.FirstBehavior().hyperparameters.ApplyNew(hyperParameter);
                HyperParameterBridge.Write(newParams, hyperParamPath);
            }
            else
            {
                Debug.LogWarning("Could not parse suggested hyper parameters!");
            }
            
            // To Write the new hyper parameters
            ProcessNextPoint();
        }

        public void StoreCode(string response)
        {
            codeResponse = response;
            ProcessNextPoint();
        }
        
        public void StartCodeDirectly(string response)
        {
            LoggingController.Log($" - Start Code Directly - [MODERATOR] CODE: {response}");

            cliBridge.RunMLAgentsInWSL(behaviourName, mlAgentsConfigPath);
            StartCoroutine(CompileDelay(response));
            ConversationModerator.RUNNING = true;
        }
        
        public void StartCode()
        {
            cliBridge.RunMLAgentsInWSL(behaviourName, mlAgentsConfigPath); //Start it before compiling and running the code, as it might take a while to spin up the process
            StartCoroutine(CompileDelay(codeResponse)); //Wait a bit for the ml-agents to start up
            ConversationModerator.RUNNING = true;
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
            
            gptConverser.SinglePrompt(response);
        }

        private void GroundedObservationResponse(List<Kosmos2GroundedRelation> groundedRelations)
        {
            StringBuilder relationDescription = new StringBuilder();
            foreach (var relation in groundedRelations)
            {
                relationDescription.Append(
                    $"{relation.label} could be found by the name {relation.subject.name} in Unity's hierarchy. It contains the following Components: {relation.components}. These components {relation.exposedComponents} contain the following exposed methods: {relation.exposedMethods}\n");
            }
            gptConverser.SinglePrompt(relationDescription.ToString());
            summary += relationDescription.ToString();
        }
        
    }
}