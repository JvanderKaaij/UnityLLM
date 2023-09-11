using OpenAIGPT;
using UnityEngine;

public class GPTCodeTester : MonoBehaviour
{
    [SerializeField] GPTCSharpBridge bridge;
    
    [TextAreaAttribute (10, 100)]
    [SerializeField] private string code;
    
    public static GPTCodeTester Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    
    [ContextMenu("Execute")]
    public void Execute()
    {
        bridge.Process(code);
    }
}
