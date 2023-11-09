using System;
using OpenAIGPT;
using TMPro;
using UnityEngine;

public class InputUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private UnityEngine.UI.Button button;
    [SerializeField] private Connector connector;
    [SerializeField] private GPTConverser converser;
    [SerializeField] private TextMeshProUGUI responseText;
    private void Start()
    {
        button.onClick.AddListener(OnSend);
    }

    private void OnSend()
    {
        Debug.Log("Press Button");
        converser.SinglePrompt(inputField.text);
        converser.OnResponse.AddListener(resp => responseText.text = resp);
    }
    
}
