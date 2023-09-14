using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CLIBridge : MonoBehaviour
{
    [SerializeField]
    private string MLAgentsVenvPath = "/usr/local/pyenv/versions/virtualenv/bin/activate";

    public Action ProcessDone;
    
    private ConcurrentQueue<Action> mainThreadActions = new();

    
    private void Update()
    {
        while(mainThreadActions.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }

    public async void RunMLAgentsInWSL(string behaviourName, string configPath)
    {
        await Task.Run(() => ExecuteMLAgentsProcess(behaviourName, configPath));
    }

    void ExecuteMLAgentsProcess(string behaviourName, string configPath)
    {
        /*
         * This is the code that you need to run in order to start the ml-agents-learn command
         * Note that the pyenv virtualenv must be activated before running the command
         * You can retrieve the location by running pyenv prefix in the folder that the pyenv runs in
         */
        
        string commandToRun = $"mlagents-learn {configPath} --run-id={behaviourName} --force";
        
        Debug.Log("Start ML-Agents Process");
        LoggingController.Log("START ML-Agents Process");

        RunCLIProcess(MLAgentsVenvPath, commandToRun,
        (x) =>
        {
            Debug.Log(x);
        },
        () =>
        {
            ProcessDone?.Invoke();
            LLMRLMetaController.OnNextStep();
            Debug.Log("ML-Agents Process Ended");
        });
    }

    private void RunCLIProcess(string venvPath, string commandToRun, Action<string> OnData, Action OnDone)
    {
        string wslCommand = $"wsl source {venvPath} ; {commandToRun}";

        // Set up the process start info
        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + wslCommand)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        // Start the process
        Process process = new Process { StartInfo = psi };
        
        process.OutputDataReceived += (sender, data) =>
        {
            if (!String.IsNullOrEmpty(data.Data))
            {
                OnData(data.Data.Trim());
            }
        };
            
        process.ErrorDataReceived += (sender, data) =>
        {
            if (!String.IsNullOrEmpty(data.Data))
            {
                Debug.Log("ERROR: " + data.Data);
            }
        };
        
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        
        //As we're in another Thread I need to Queue the Action into the mainThread
        mainThreadActions.Enqueue(OnDone);
    }
    
}