using System.Collections.Generic;
using System.Text;
using Kosmos2;
using OpenAIGPT;
using UnityEngine;

namespace DefaultNamespace
{
    public class ConversationModerator:MonoBehaviour
    {
        [SerializeField] LLMOObserver observer;
        [SerializeField] GPTConverser gptConverser;
        [SerializeField] GPTCSharpBridge codeBridge;

        private int counter;
        
        private void Start()
        {
            gptConverser.CallMessage("Ask your first question");
            gptConverser.OnResponse.AddListener(GPTResponse);
            observer.OnResponse.AddListener(ObserverResponse);
            observer.OnGroundedResponse.AddListener(GroundedObservationResponse);
        }

        private void GPTResponse(string response)
        {
            counter++;
            if(counter <= 10){
                observer.Observe(response);
            }else if (counter == 11)
            {
                gptConverser.CallMessage("Now give a detailed description of the complete scene.");
            }else if (counter == 12){
                observer.ObserveGrounded();
            }else if (counter == 13) {
                gptConverser.CallMessage("The image represents a Unity scene, and all the objects are objects in the scene. What could be a possible thing that could happen in this scene to one of the objects?");
            }else if (counter == 14){
                gptConverser.CallMessage("Write me Unity C# code that makes what you just described happen. Don't require user interaction. Use only the existing components. Never use any prefabs. Always find the objects in the hierarchy by GameObject. Find in the Start function. For updating use the Update function.");
            }else if (counter == 15){
                codeBridge.Process(response);
            }
        }

        private void ObserverResponse(string response)
        {
            string last = gptConverser.GetLastMessage();
            //remote last string from response
            response = response.Replace(last, "");
            gptConverser.CallMessage(response);
        }

        private void GroundedObservationResponse(List<Kosmos2GroundedRelation> groundedRelations)
        {
            StringBuilder relationDescription = new StringBuilder();
            foreach (var relation in groundedRelations)
            {
                relationDescription.Append(
                    $"{relation.label} could be find by the name {relation.subject.name} in the Unity Hierarchy. It contains the following Components: {relation.components}");
            }
            gptConverser.CallMessage(relationDescription.ToString());
        }
        
    }
}