using System.Collections.Generic;
using UnityEngine;

namespace OpenAIGPT
{
    public class GPTConverser:MonoBehaviour
    {
        [SerializeField] private GPTConnector connector;
        [SerializeField] private List<GPTMessageData> messagesArray = new();

        //TODO There is no standard memory of what has been said. That needs to be implemented.
        //TODO Make sure that the order of messages is maintained.
        [ContextMenu("Message")]
        private void Message()
        {
            messagesArray.Add(new GPTMessageData { role = "user", content = "My name is Joey" });
            StartCoroutine(connector.SendWebRequest(messagesArray.ToArray(), AssistantResponse));
        }
        
        [ContextMenu("Message 2")]
        private void Message2()
        {
            messagesArray.Add(new GPTMessageData { role = "user", content = "What is my name?" });
            StartCoroutine(connector.SendWebRequest(messagesArray.ToArray(), AssistantResponse));
        }
        
        public void AssistantResponse(GPTResponseData responseData){
            Debug.Log(responseData);
            messagesArray.Add(responseData.choices[0].message);
            // Debug.Log(responseData.choices[0].message.content);
        }
        
    }
}