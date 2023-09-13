﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;

namespace Kosmos2
{
    [Serializable]
    public class KosmosResponseData
    {
        public string message;
        public List<DetectedObject> entities;
    }
    
    [Serializable]
    public class BoundingBox
    {
        public Vector2 position;
        public float x_min;
        public float y_min;
        public float x_max;
        public float y_max;
    }

    [Serializable]
    public class DetectedObject
    {
        public string label;
        public Vector2Int boundingBoxPosition;
        public BoundingBox boundingBox;
    }
    
    public class Kosmos2Connector:MonoBehaviour
    {
        [SerializeField] private string apiUrl = "http://localhost:8005/process_grounding_prompt";
        public IEnumerator InterpretImage(byte[] imageBytes, string prompt, Action<KosmosResponseData> ResponseCallback)
        {
            WWWForm form = new WWWForm();
            form.AddBinaryData("image", imageBytes);
            form.AddField("prompt", prompt);
            
            LoggingController.Log($"[KOSMOS] [user] {prompt}");

            using (UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl, form))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Kosmos Connection Error: {webRequest.error}");
                }
                else
                {
                    KosmosResponseData response = JsonUtility.FromJson<KosmosResponseData>(webRequest.downloadHandler.text);
                    response.entities.ForEach(x => x.boundingBox.position = x.boundingBoxPosition);
                    ResponseCallback(response);
                    LoggingController.Log($"[KOSMOS] [reponse] {response.message}");
                }
            }

            yield return null;
        }
        
    }
}