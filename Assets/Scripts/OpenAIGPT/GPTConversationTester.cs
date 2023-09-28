using OpenAIGPT;
using UnityEngine;

public class GPTConversationTester : MonoBehaviour
{
   [TextAreaAttribute]
   [SerializeField] private string message;

   [SerializeField] private GPTConverser converser;
   
   [ContextMenu("Send Text")]
   public void SendText()
   {
      converser.Prompt(message);
      message = "";
   }

   public void Response(string response)
   {
      Debug.Log($"GPT Response: {response}");
   }
}
