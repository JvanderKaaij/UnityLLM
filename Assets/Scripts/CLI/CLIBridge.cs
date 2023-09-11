using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CLIBridge : MonoBehaviour
{
    [SerializeField]
    private string virtualEnvPath = "/usr/local/pyenv/versions/virtualenv/bin/activate";
    [SerializeField]
    private string runID = "Ballance";

    public async void RunMLAgentsInWSL(string configPath)
    {
        await Task.Run(() => ExecuteProcess(configPath));
    }
    
    void ExecuteProcess(string configPath)
    {
        /*
         * This is the code that you need to run in order to start the ml-agents-learn command
         * Note that the pyenv virtualenv must be activated before running the command
         * You can retrieve the location by running pyenv prefix in the folder that the pyenv runs in
         */
        
        Debug.Log($"Starting mlagents with configpath: {configPath} and runID: {runID}");
        
        string commandToRun = $"mlagents-learn {configPath} --run-id={runID} --force";
        
        // Formulate the WSL command to activate pyenv virtualenv and then run the command
        string wslCommand = $"wsl source {virtualEnvPath} ; {commandToRun}";

        // Set up the process start info
        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + wslCommand)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        
        Debug.Log("Start Process");
        
        // Start the process
        Process process = new Process { StartInfo = psi };
        process.Start();
        
        string output = process.StandardOutput.ReadToEnd();
        string errorOutput = process.StandardError.ReadToEnd();

        // Log the output and errors
        if (!string.IsNullOrEmpty(output))
            Debug.Log(output);

        if (!string.IsNullOrEmpty(errorOutput))
            Debug.LogError(errorOutput);

        process.WaitForExit();

        Debug.Log("Process Ended");
    }
}