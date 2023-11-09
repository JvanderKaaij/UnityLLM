using System;
using System.Collections.Generic;
using System.Linq;
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
        
        [SerializeField] private string gpt_model = "gpt-4"; //gpt-3.5-turbo";
        
        [SerializeField] private string gpt_image_model = "gpt-4-vision-preview"; //gpt-3.5-turbo";
        
        public UnityEvent<string> OnResponse;
        private void Awake()
        {
            SinglePrompt(preContext, "system");
        }
        
        public void Prompt(string content)
        {
            SinglePrompt(content);
        }
        
        public void SinglePrompt(string content, string role="user")
        {
            Debug.Log($"Try and Prompt with {content}");
            var contents = new List<GPTMessageContentType> { new GPTMessageContentType() { type = "text", text = content }};
            messagesArray.Add(new GPTMessageData { role = role, content = contents});
            StartCoroutine(connector.SendWebRequest(messagesArray.ToArray(), AssistantResponse, gpt_image_model) );
        }
        
        public void PromptImage(byte[] image)
        {
            //Note that it's possible to send more images at the same time
            
            string imageEncoded = Convert.ToBase64String(image);
            Debug.Log(imageEncoded);
            GPTImageURL image_url = new GPTImageURL{url=$"data:image/png;base64,{imageEncoded}"};
            var contents = new List<GPTMessageContentType> { new GPTMessageContentType() { type = "image_url", image_url = image_url }};
            messagesArray.Add(new GPTMessageData { role = "user", content = contents });
            StartCoroutine(connector.SendWebRequest(messagesArray.ToArray(), AssistantResponse, gpt_image_model) );
        }
        
        //Removes previous conversation, places a summary and possible previous data. To prepare for code and hyper parameter request
        public void PrepareSummary(TrainingSummary summary)
        {
            messagesArray = new List<GPTMessageData>();
            SinglePrompt(afterSummaryContext, "system");
            StringBuilder summaryText = new StringBuilder();

            summaryText.Append($"{summary.contextSummary}\n");
            
            // DISABLED CODE HISTORY AS IT'S STEERING TOO MUCH
            
            // if(summary.previousCode?.Length > 0){
            //     summaryText.Append($"{previousCodePrompt}: {summary.previousCode}\n");
            // }
            
            //TODO Add previous errors
            summaryText.Append($"The previous generated C# code had the following errors: {summary.previousCodeError}. Avoid them this time.\n");

            if (summary.previousTensorData != null && summary.previousTensorData.HasData())
            {
                summaryText.Append($"{previousTensorDataPrompt}\n");
                summaryText.Append(summary.previousTensorData);
            }

            summaryText.Append($"\n{previousHyperParameterPrompt}\n");
            summaryText.Append(summary.previousHyperParams);
            
            Debug.Log("Constructed Summary:");
            Debug.Log(summaryText.ToString());
            
            LoggingController.Log($"[SUMMARY]: {summaryText}");
            SinglePrompt(summaryText.ToString());
        }

        public string GetLastMessage()
        {
            //TODO Not sure of this yet! What is this used for again?
            // return messagesArray[messagesArray.Count - 1].content.Last().text;
            return "Not used atm";
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
            messagesArray.Add(responseData.choices[0].message.ToGPTMessageData());
            OnResponse.Invoke(responseData.choices[0].message.content);//TODO I think this is fine as it's only a response
        }
    }
}