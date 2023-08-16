using System;
using UnityEngine;

public class InputUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField inputField;
    [SerializeField] private UnityEngine.UI.Button button;
    [SerializeField] private Connector connector;
    
    private void Start()
    {
        button.onClick.AddListener(OnSend);
    }

    private void OnSend()
    {
        connector.Post(inputField.text);
    }
    
}
