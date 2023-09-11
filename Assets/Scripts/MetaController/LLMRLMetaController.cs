using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private CLIBridge CLIBridge;
        public static List<GPTMessageData> fullConversation = new();
        private bool continueLoop = true;

        static LLMRLMetaController mInstance;
 
        public static LLMRLMetaController Instance
        {
            get
            {
                if (mInstance == null)
                {
                    Academy.OnEpochReset += OnNextStep;
                    RuntimeCompiler.CompilerError += OnRestart;
                    Application.logMessageReceived += OnLog;
                    mInstance = new();
                }
                return mInstance;
            }
        }
        
        public static void OnNextStep()
        {
            Instance.NextStep();
        }
        
        public static void OnLog(string logString, string stackTrace, LogType type)
        {
            Instance.HandleLog(logString, stackTrace, type);
        }
        
        public static void OnRestart()
        {
            Instance.RestartTraining();
        }
        
        void OnEnable() {
            Debug.Log("ENABLED!");
            CLIBridge.ProcessDone += OnNextStep;
        }

        void OnDisable() {
            Debug.Log("DISABLED");
            Application.logMessageReceived -= HandleLog;
        }
        
        void HandleLog(string logString, string stackTrace, LogType type) {
        
            if (type == LogType.Error) {
               Debug.Log("Error Received!!! , Let's Restart");
               RestartTraining();
            }   
        }

        [ContextMenu("Start Training")]
        private void StartTraining()
        {
            EditorApplication.isPlaying = true;
        }

        public void RestartTraining()
        {
            Debug.Log("RESTART");
            EditorApplication.isPlaying = false;
            EditorCoroutine.Run(Restart());
        }
        
        public void NextStep()
        {
            Debug.Log($"GOT NEXT STEP! {fullConversation.Count}");
            EditorCoroutine.Run(Restart());
        }
        private IEnumerator Restart()
        {
            yield return new WaitForEndOfFrame();
            EditorApplication.isPlaying = false;
            yield return new WaitForSeconds(2.0f);
            Debug.Log("Waited 2 seconds");
            EditorApplication.isPlaying = true;
        }

    }
}