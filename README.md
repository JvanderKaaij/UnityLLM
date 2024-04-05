## Unity LLM
This project contains the elements for driving Reinforcement Learning scenario through conversation with a Large Language model.

The idea is that through GPTConnector and GPTConverser a prompt/conversation takes place and when code arrives, it will be compiled.

Confusingly there is some leftover code not being in use anymore (e.g. the Kosmos2 VQA has been taken out after GPT4 started supporting VQA)

For info on the reinforcement learning part look at Unity's ML-Agents package. 

**IMPORTANT!**
The runtime compiler in this project needs to have the right settings in RuntimeCompiler.cs:
To be honest I just played with importing the references I thought it needed from certain locations until it worked.
```
// System namespace for common types like collections.
param.ReferencedAssemblies.Add("System.dll");
param.ReferencedAssemblies.Add("D:/UserProjects/Joey/Unity/UnityLLM/Library/ScriptAssemblies/Assembly-CSharp.dll");
param.ReferencedAssemblies.Add("C:/Program Files/Unity/Hub/Editor/2022.3.0f1/Editor/Data/Managed/UnityEngine/UnityEngine.dll");
param.ReferencedAssemblies.Add("C:/Program Files/Unity/Hub/Editor/2022.3.0f1/Editor/Data/Managed/UnityEngine/UnityEngine.InputLegacyModule.dll");
param.ReferencedAssemblies.Add("C:/Program Files/Unity/Hub/Editor/2022.3.0f1/Editor/Data/Managed/UnityEngine/UnityEngine.InputModule.dll");
param.ReferencedAssemblies.Add("C:/Program Files/Unity/Hub/Editor/2022.3.0f1/Editor/Data/Managed/UnityEngine/UnityEngine.PhysicsModule.dll");
param.ReferencedAssemblies.Add("D:/UserProjects/Joey/Unity/AssemblyGPT/Assets/Unity-Roslyn/netstandard.dll");
param.ReferencedAssemblies.Add("D:/UserProjects/Joey/Unity/UnityLLM/Library/ScriptAssemblies/Unity.ML-Agents.dll");
param.ReferencedAssemblies.Add(typeof(MonoBehaviour).Assembly.Location);
```

