using System.Collections.Generic;
using System.Text;
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
        
        public UnityEvent<string> OnResponse;
        private void Awake()
        {
            messagesArray.Add(new GPTMessageData { role = "user", content = preContext});
        }
        
        public void CallMessage(string content)
        {
            Debug.Log($"Ask GPT: {content}");
            messagesArray.Add(new GPTMessageData { role = "user", content = content });
            StartCoroutine(connector.SendWebRequest(messagesArray.ToArray(), AssistantResponse));
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
            Debug.Log($"GPT Response: {responseData.choices[0].message.content}");
            Debug.Log($"Finish Reason: {responseData.choices[0].finish_reason}");
            Debug.Log($"Usage: prompt token: {responseData.usage.prompt_tokens} completion tokens: {responseData.usage.completion_tokens} total tokens: {responseData.usage.total_tokens}");
            
            messagesArray.Add(responseData.choices[0].message);
            OnResponse.Invoke(responseData.choices[0].message.content);
            
            // bridge.Process(responseData.choices[0].message.content);
        }

    }
}