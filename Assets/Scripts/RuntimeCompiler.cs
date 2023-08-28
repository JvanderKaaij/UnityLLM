using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using UnityEngine;

public class RuntimeCompiler : MonoBehaviour
{
    public void Call(string code, string className)
    {
        var assembly = Compile(code);
        
        var runtimeType = assembly.GetType(className);
        var method = runtimeType.GetMethod("AddYourselfTo");
        var del = (Func<GameObject, MonoBehaviour>)
                      Delegate.CreateDelegate(
                          typeof(Func<GameObject, MonoBehaviour>), 
                          method
                  );

        // We ask the compiled method to add its component to this.gameObject
        var addedComponent = del.Invoke(gameObject);

        // The delegate pre-bakes the reflection, so repeated calls don't
        // cost us every time, as long as we keep re-using the delegate.
    }

    private static Assembly Compile(string source)
    {
        // Replace this Compiler.CSharpCodeProvider wth aeroson's version
        // if you're targeting non-Windows platforms:
        var provider = new CSharpCodeProvider();
        var param = new CompilerParameters();
        
        // System namespace for common types like collections.
        param.ReferencedAssemblies.Add("System.dll");
        param.ReferencedAssemblies.Add("C:/Program Files/Unity/Hub/Editor/2022.3.0f1/Editor/Data/Managed/UnityEngine/UnityEngine.dll");
        param.ReferencedAssemblies.Add("C:/Program Files/Unity/Hub/Editor/2022.3.0f1/Editor/Data/Managed/UnityEngine/UnityEngine.InputLegacyModule.dll");
        param.ReferencedAssemblies.Add("C:/Program Files/Unity/Hub/Editor/2022.3.0f1/Editor/Data/Managed/UnityEngine/UnityEngine.InputModule.dll");
        param.ReferencedAssemblies.Add("C:/Program Files/Unity/Hub/Editor/2022.3.0f1/Editor/Data/Managed/UnityEngine/UnityEngine.PhysicsModule.dll");
        param.ReferencedAssemblies.Add ("D:/UserProjects/Joey/Unity/AssemblyGPT/Assets/Unity-Roslyn/netstandard.dll");
        param.ReferencedAssemblies.Add (typeof(MonoBehaviour).Assembly.Location);
        
        // foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
        //     param.ReferencedAssemblies.Add(assembly.Location);
        // } 

        // Generate a dll in memory
        param.GenerateExecutable = false;
        param.GenerateInMemory = true;

        // Compile the source
        var result = provider.CompileAssemblyFromSource(param, source);

        if (result.Errors.Count > 0) {
            var msg = new StringBuilder();
            bool canContinue = true; //continue if all errors are just warnings.
            foreach (CompilerError error in result.Errors) {
                if(!error.IsWarning) canContinue = false;
                msg.AppendFormat("Error ({0}): {1} LineNumber: {2}\n",
                    error.ErrorNumber, error.ErrorText, error.Line);
            }
            if(!canContinue) throw new Exception(msg.ToString());
        }

        // Return the assembly
        return result.CompiledAssembly;
    }
}