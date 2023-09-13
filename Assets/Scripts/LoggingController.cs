using System;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class LoggingController:MonoBehaviour
    {
        [SerializeField] private string path;

        private string filename;
        static LoggingController Instance;
 
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Instance.filename = $"logging_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        public static void Log(string log)
        {
            var text = $"{DateTime.Now}: {log}\n";
            File.AppendAllText($"{Instance.path}/{Instance.filename}", text);
        }
    }
}