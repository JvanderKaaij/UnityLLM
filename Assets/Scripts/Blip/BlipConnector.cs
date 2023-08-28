using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Blip
{
    [System.Serializable]
    public class GeneratedTextData
    {
        public string generated_text;
    }

    [System.Serializable]
    public class RootData
    {
        public List<GeneratedTextData> root;
    }
    
    public class BlipConnector:MonoBehaviour
    {
        [SerializeField] private string apiUrl = "https://api-inference.huggingface.co/models/Salesforce/blip-image-captioning-large";

        public IEnumerator InterpretImage(byte[] imageBytes, Action<String> ResponseCallback)
        {
            UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(imageBytes);
            uploadHandler.contentType = "image/png"; // Replace with the appropriate content type
            request.uploadHandler = uploadHandler;
            
            DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();
            request.downloadHandler = downloadHandler;
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = downloadHandler.text;
                Debug.Log("{\"root\":" + responseText + "}");
                RootData dataList = JsonUtility.FromJson<RootData>("{\"root\":" + responseText + "}");
                ResponseCallback(dataList.root[0].generated_text);
            }
            else
            {
                Debug.LogError("Image upload failed: " + request.error);
            }

            yield return null;
        }
    }
}