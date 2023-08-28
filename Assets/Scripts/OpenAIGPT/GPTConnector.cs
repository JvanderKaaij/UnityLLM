using System;
using System.Collections;
using System.Text;
using CandyCoded.env;
using UnityEngine;
using UnityEngine.Networking;

namespace OpenAIGPT
{
    public class GPTConnector : MonoBehaviour
    {
        [SerializeField] private string apiUrl = "https://api.openai.com/v1/chat/completions";
        [SerializeField] private string model = "gpt-4"; //gpt-3.5-turbo";

        public IEnumerator SendWebRequest(GPTMessageData[] message, Action<GPTResponseData> ResponseCallback)
        {
            GPTMessageWrapper wrapper = new GPTMessageWrapper
            {
                model = model,
                temperature = 0.7f,
                messages = message
            };

            string messagesJson = JsonUtility.ToJson(wrapper);

            // Debug.Log(messagesJson);

            using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(apiUrl, ""))
            {
                //These are stored in the .env file in the root of the project
                env.TryParseEnvironmentVariable("OPENAI_API_KEY", out string authorizationToken);
                env.TryParseEnvironmentVariable("OPENAI_ORG_ID", out string organisationID);
                
                // Add Authorization header
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", $"Bearer {authorizationToken}");
                webRequest.SetRequestHeader("OpenAI-Organization", organisationID);

                byte[] jsonBytes = Encoding.UTF8.GetBytes(messagesJson);
                webRequest.uploadHandler = new UploadHandlerRaw(jsonBytes);

                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for errors
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Web request failed: " + webRequest.error);
                }
                else
                {
                    // Process the response
                    GPTResponseData response = JsonUtility.FromJson<GPTResponseData>(webRequest.downloadHandler.text);
                    ResponseCallback(response);
                }
            }

            yield return null;
        }

    }
}