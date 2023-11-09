using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using CandyCoded.env;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;

namespace OpenAIGPT
{
    public class GPTConnector : MonoBehaviour
    {
        [SerializeField] private string apiUrl = "https://api.openai.com/v1/chat/completions";
        [SerializeField] private float temperature = 0.7f;
        [SerializeField] private int maxTokens = 3000;

        public IEnumerator SendWebRequest(GPTMessageData[] messages, Action<GPTResponseData> ResponseCallback, string model)
        {
            GPTMessageWrapper wrapper = new GPTMessageWrapper
            {
                model = model,
                temperature = temperature,
                messages = messages,
                max_tokens = maxTokens
            };

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string messagesJson = JsonConvert.SerializeObject(wrapper, settings);

            // string messagesJson = JsonUtility.ToJson(wrapper);

            Debug.Log($" Full Message JSON To Send: {messagesJson}");
            
            //TODO Fix this again to log correctly
            LoggingController.Log($" - Before API Request - [{model}] [{wrapper.messages.Last().role}] {wrapper.messages.Last().content[0].text}");
            
            using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(apiUrl, ""))
            {
                //These are stored in the .env file in the root of the project
                env.TryParseEnvironmentVariable("OPENAI_API_KEY", out string authorizationToken);
                env.TryParseEnvironmentVariable("OPENAI_ORG_ID", out string organisationID);
                
                // Add Authorization header
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", $"Bearer {authorizationToken}");
                webRequest.SetRequestHeader("OpenAI-Organization", organisationID);
                webRequest.SetRequestHeader("OpenAI-Organization", organisationID);

                byte[] jsonBytes = Encoding.UTF8.GetBytes(messagesJson);
                webRequest.uploadHandler = new UploadHandlerRaw(jsonBytes);

                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for errors
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Web request failed: {webRequest.error}: {webRequest.downloadHandler.text}");
                }
                else
                {
                    // Process the response
                    Debug.Log("On GPT Respond");
                    GPTResponseData response = JsonUtility.FromJson<GPTResponseData>(webRequest.downloadHandler.text);
                    LoggingController.Log($" - On API Response - [{response.choices[0].message.role}]: {response.choices[0].message.content}");
                    ResponseCallback?.Invoke(response);
                }
            }

            yield return null;
        }

    }
}