using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private bool grounding = true;
        public IEnumerator InterpretImage(byte[] imageBytes, string prompt, Action<KosmosResponseData> ResponseCallback)
        {
            WWWForm form = new WWWForm();
            form.AddBinaryData("image", imageBytes);
            
            string promptType = grounding ? $"<grounding>{prompt}" : prompt;
            form.AddField("prompt", promptType);
            
            using (UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl, form))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    KosmosResponseData response = JsonUtility.FromJson<KosmosResponseData>(webRequest.downloadHandler.text);
                    response.entities.ForEach(x => x.boundingBox.position = x.boundingBoxPosition);
                    ResponseCallback(response);
                }
            }

            yield return null;
        }
        
    }
}