using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;

namespace TensorData
{
    public class TensorDataConnector : MonoBehaviour
    {
        [SerializeField] private string apiUrl = "http://localhost:8006/request";

        public IEnumerator RequestTensorData(string path, Action<TensorDataList> ApplySummary)
        {
            WWWForm form = new WWWForm();
            form.AddField("path", path);
            
            LoggingController.Log($"[TensorData] [user] {path}");

            using (UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl, form))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"TensorData Connection Error: {webRequest.error}");
                }
                else
                {
                    TensorDataList response = JsonUtility.FromJson<TensorDataList>(webRequest.downloadHandler.text);
                    ApplySummary.Invoke(response);
                    LoggingController.Log($"[TensorData] [reponse] {response.message}");
                }
            }

            yield return null;
        }

    }
}