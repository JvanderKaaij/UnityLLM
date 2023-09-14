using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteInEditMode]
    [InitializeOnLoad]
    public class LLMRLMetaController:MonoBehaviour
    {
        public static string currentCode;
        public static string currentSummary;

        public string sessionPath = "D:/UserProjects/Joey/Unity/UnityLLM/results/session.json";
        
        private bool continueLoop = true;

        static LLMRLMetaController mInstance;
 
        public static LLMRLMetaController Instance
        {
            get
            {
                if (mInstance == null)
                {
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
        
        void OnDisable() {
            Application.logMessageReceived -= HandleLog;
        }
        
        void HandleLog(string logString, string stackTrace, LogType type) {
        
            if (type == LogType.Error) {
               Debug.Log("Error Received, Let's Restart");
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
            Debug.Log($"GOT NEXT STEP");
            StoreData(); //TODO can be moved to happen after ConversationModerator generated them?
            //With previous code, previous hyper params and monitoring, start a new run
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

        [ContextMenu("Test Store Data")]
        private void StoreData()
        {
            var data = MetaSessionDataController.RetrieveSessionData(Instance.sessionPath);
            data.codeHistory.Add(currentCode);
            data.summaryHistory.Add(currentSummary);
            MetaSessionDataController.Save(Instance.sessionPath, data);
        }

    }
}