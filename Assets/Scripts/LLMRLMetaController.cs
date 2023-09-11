using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenAIGPT;
using Unity.MLAgents;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteInEditMode]
    [InitializeOnLoad]
    public class LLMRLMetaController:MonoBehaviour
    {
        [SerializeField] private GPTConverser GPTConversation;
        private List<GPTMessageData> fullConversation = new();
        private bool continueLoop = true;

        LLMRLMetaController()
        {
            // Academy.Instance.OnEpochReset += NextStep;
        }
        
        void OnEnable() {
            Debug.Log("ENABLED!");
            Application.logMessageReceived += HandleLog;
            Academy.Instance.OnEpochReset += NextStep;
        }

        void OnDisable() {
            Application.logMessageReceived -= HandleLog;
            Academy.Instance.OnEpochReset -= NextStep;

        }
        
        void HandleLog(string logString, string stackTrace, LogType type) {
        
            if (type == LogType.Error) {
               Debug.Log("Error Received!!! , Let's Restart");
            }   
        }

        
        [ContextMenu("Start Training")]
        private void StartTraining()
        {
            EditorApplication.isPlaying = true;
        }

        public void NextStep()
        {
            // fullConversation.AddRange(GPTConversation.messagesArray);
            Debug.Log($"GOT NEXT STEP! {fullConversation.Count}");
            EditorApplication.isPlaying = false;
            EditorCoroutine.Run(Restart());
        }
        private IEnumerator Restart()
        {
            yield return new WaitForSeconds(2.0f);
            Debug.Log("Waited 2 seconds");
            EditorApplication.isPlaying = true;
        }

    }
}