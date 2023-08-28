using System;
using OpenAIGPT;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LLMObservation
{
    public class LLMOBridge:MonoBehaviour
    {
        [SerializeField] private GPTConnector connector;

        private void Start()
        {
        }
    }
}