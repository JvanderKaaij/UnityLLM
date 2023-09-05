using OpenAIGPT;
using UnityEngine;

public class GPTCodeTester : MonoBehaviour
{
    [SerializeField] GPTCSharpBridge bridge;
    
    [TextAreaAttribute (10, 100)]
    [SerializeField] private string code;

    [ContextMenu("Execute")]
    private void Execute()
    {
        bridge.Process(code);
    }
}
