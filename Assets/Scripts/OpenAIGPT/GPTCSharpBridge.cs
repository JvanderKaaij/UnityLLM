using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace OpenAIGPT
{
    public class GPTCSharpBridge:MonoBehaviour
    {
        [SerializeField] private RuntimeCompiler compiler;

        public void Process(string response)
        {
            var codeExtract = ExtractCode(response);
            var className = GetClassName(response);
            var completeCodeExtract = AddMeta(codeExtract, className);
            Debug.Log(completeCodeExtract);
            compiler.Call(completeCodeExtract, className);
        }

        private string GetClassName(string response)
        {
            string pattern = @"class\s+(\w+)\s*:";

            // Use Regex.Match to find the class name
            Match match = Regex.Match(response, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                Debug.LogError("Class name not found.");
                return null;
            }
        }
        
        
        private string AddMeta(string code, string className)
        {
            var callFunction =
                @"public static {className} AddYourselfTo(GameObject host){ return host.AddComponent<{className}>();}";
            
            callFunction = callFunction
                .Replace("{className}", className);
            
            int classEndIndex = code.LastIndexOf("}");
            string updatedClass = code.Insert(classEndIndex, callFunction);
            return updatedClass;
        }
        
        private string ExtractCode(string response)
        {
            string pattern = @"```csharp([\s\S]*?)```";
            Match match = Regex.Match(response, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }
            else
            {
                if (response.Contains("MonoBehaviour"))// might be it's just code and nothing else.
                {
                    return response;
                }
                else
                {
                    return "No c# code found.";
                }
            }
        }
    }
}