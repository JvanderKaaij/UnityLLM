using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace OpenAIGPT
{
    public class GPTConverser:MonoBehaviour
    {
        [SerializeField] private GPTConnector connector;
        // [SerializeField] private GPTCSharpBridge bridge;
        [SerializeField] public List<GPTMessageData> messagesArray = new();
        
        [TextAreaAttribute]
        [SerializeField] private string preContext;
        
        [TextAreaAttribute]
        [SerializeField] private string afterSummaryContext;
        
        [TextAreaAttribute]
        [SerializeField] private string previousCodePrompt;
        
        [TextAreaAttribute]
        [SerializeField] private string previousTensorDataPrompt;
        
        [TextAreaAttribute]
        [SerializeField] private string previousHyperParameterPrompt;
        
        public UnityEvent<string> OnResponse;
        private void Awake()
        {
            messagesArray.Add(new GPTMessageData { role = "system", content = preContext});
        }
        
        public void Prompt(string content)
        {
            messagesArray.Add(new GPTMessageData { role = "user", content = content });
            StartCoroutine(connector.SendWebRequest(messagesArray.ToArray(), AssistantResponse) );
        }
        
        //Removes previous conversation, places a summary and possible previous data. To prepare for code and hyper parameter request
        public void PrepareSummary(TrainingSummary summary)
        {
            messagesArray = new List<GPTMessageData>();
            messagesArray.Add(new GPTMessageData { role = "system", content = afterSummaryContext});
            StringBuilder summaryText = new StringBuilder();
            summaryText.Append($"{previousCodePrompt}: {summary.previousCode}");

            if (summary.previousTensorData.HasData())
            {
                summaryText.Append(previousTensorDataPrompt);
                summaryText.Append(summary.previousTensorData);
            }

            summaryText.Append(previousHyperParameterPrompt);
            summaryText.Append(summary.previousHyperParams);
            
            Debug.Log("Constructed Summary:");
            Debug.Log(summaryText.ToString());
            messagesArray.Add(new GPTMessageData{ role = "user", content = summaryText.ToString()});
        }

        public string GetLastMessage()
        {
            return messagesArray[messagesArray.Count - 1].content;
        }
        
        [ContextMenu("Export")]
        private void Export()
        {
            StringBuilder strb = new StringBuilder();
            foreach (var msg in messagesArray)  
            {
                strb.Append($"____ ***{msg.role}***\n{msg.content}\n");
            }
            GUIUtility.systemCopyBuffer = strb.ToString();
            Debug.Log(strb.ToString());
        }
        
        public void AssistantResponse(GPTResponseData responseData){
            Debug.Log($"Usage: prompt token: {responseData.usage.prompt_tokens} completion tokens: {responseData.usage.completion_tokens} total tokens: {responseData.usage.total_tokens}");
            messagesArray.Add(responseData.choices[0].message);
            OnResponse.Invoke(responseData.choices[0].message.content);
        }
    }
}