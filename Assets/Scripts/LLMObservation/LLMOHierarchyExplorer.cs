using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace LLMObservation
{
    public class LLMOHierarchyExplorer:MonoBehaviour
    {
        public string ObjectDescription(GameObject obj)
        {
            string transform = GetTransformDescription(obj);
            // string components = GetComponentDescription(obj);
            string exposedMethods = GetExposedMethods(obj);
            return $"The object named {obj.name} has the following transform: {transform}. It contains the following exposed methods: {exposedMethods} \n";
        }

        public string HierarchyDescription()
        {
            StringBuilder sb = new StringBuilder();
            foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
            {
                sb.Append(ObjectDescription(obj));
            }

            return sb.ToString();
        }

        private string GetTransformDescription(GameObject obj)
        {
            string position = $"x: {obj.transform.position.x}, y:{obj.transform.position.y}, z:{obj.transform.position.z}";
            string rotation = $"x: {obj.transform.rotation.x}, y:{obj.transform.rotation.y}, z:{obj.transform.rotation.z}";
            string scale = $"x: {obj.transform.localScale.x}, y:{obj.transform.localScale.y}, z:{obj.transform.localScale.z}";
            return $"position: {position}, rotation: {rotation}, scale: {scale}";
        }

        public string GetComponentDescription(GameObject obj)
        {
            Component[] components = obj.GetComponents<Component>();
            
            StringBuilder sb = new StringBuilder();

            foreach (Component component in components)
            {
                sb.Append($"{component.GetType().Name} \n"); // with public methods: {GetMethodDescriptions(component)}
            }
            return sb.ToString();
        }
        
        public string GetExposedComponents(GameObject obj)
        {

            Component[] components = obj.GetComponents<Component>();
            
            StringBuilder sb = new StringBuilder();

            foreach (Component component in components)
            {
                Type type = component.GetType();
                GPTExposeAttribute attribute = type.GetCustomAttribute<GPTExposeAttribute>();

                if (attribute != null)
                {
                    sb.Append($"{component.GetType().Name}");
                }
            }
            return sb.ToString();
        }

        public string GetExposedMethods(GameObject obj)
        {

            Component[] components = obj.GetComponents<Component>();
            
            StringBuilder sb = new StringBuilder();

            foreach (Component component in components)
            {
                Type type = component.GetType();
                GPTExposeAttribute attribute = type.GetCustomAttribute<GPTExposeAttribute>();

                if (attribute != null)
                {
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var method in methods)
                    {
                        if (method.GetCustomAttributes(typeof(GPTExposeAttribute), false).Any())
                        {
                            ParameterInfo[] parameters = method.GetParameters();

                            string collectedParams = "";
                            for (var i = 0; i < parameters.Length; i++)
                            {
                                collectedParams += $"{parameters[i].ParameterType}";

                                if (i != parameters.Length - 1)
                                {
                                    collectedParams += ",";
                                }
                                
                            }
                            sb.Append($"{component.GetType().Name}.{method.Name}({collectedParams}) ");
                        }
                    }
                }
            }
            return sb.ToString();
        }
        
        private string GetMethodDescriptions(Component comp)
        {
            var compType = comp.GetType();
            
            StringBuilder sb = new StringBuilder();
            MethodInfo[] methods = compType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo method in methods)
            {
                sb.Append($"{method.Name}, ");
            }
            return sb.ToString();
        }
        
    }
}