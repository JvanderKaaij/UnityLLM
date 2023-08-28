using System;
using System.Linq;
using System.Text.RegularExpressions;
using MoonSharp.Interpreter;
using UnityEngine;

namespace OpenAIGPT
{
    public class GPTMoonBridge : MonoBehaviour
    {
        
        public void Call(string response)
        {
            if (DetectLuaCode(response))
            {
                RunCode(ExtractLuaCode(response));
            }
        }
        
        private static int InstantiateObject(string name, string type, float x, float y, float z)
        {
            PrimitiveType primitive = (PrimitiveType)Enum.Parse(typeof(PrimitiveType), type);
            GameObject prim = GameObject.CreatePrimitive(primitive);
            prim.name = name;
            prim.transform.position = new Vector3(x, y, z);
            return 0;
        }

        private static string ParseComponentType(string type)
        {
            return $"UnityEngine.{type}, UnityEngine";
        }
        
        private static int AddComponent(string name, string componentName)
        {
            var objects = FindObjectsOfType<GameObject>();
            Type componentType = Type.GetType(ParseComponentType(componentName));
            
            foreach (var obj in objects)
            {
                if(obj.name == name){
                    obj.AddComponent(componentType);
                }
            }
            return 0;
        }
        
        private static int RemoveObject(string name)
        {
            var objects = FindObjectsOfType<GameObject>();
            
            foreach (var obj in objects)
            {
                if(obj.name == name){
                    Destroy(obj);
                }
            }
            return 0;
        }
        
        private void RunCode(string luaCode)
        {
            Script script = new Script();
            script.Globals["Instant"] = (Func<string, string, float, float, float, int>) InstantiateObject;
            script.Globals["Remove"] = (Func<string, int>) RemoveObject;
            script.Globals["AddComponent"] = (Func<string, string, int>) AddComponent;
            script.DoString(luaCode);
        }
        
        static bool DetectLuaCode(string input)
        {
            string pattern = @"```lua";
            return Regex.IsMatch(input, pattern);
        }
        
        private string ExtractLuaCode(string input)
        {
            string pattern = @"```lua([\s\S]*?)```";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }
            else
            {
                return "No Lua code found.";
            }
        }
        
    }
}