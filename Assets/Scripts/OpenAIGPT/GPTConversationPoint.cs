using System;
using UnityEngine;
using UnityEngine.Events;

namespace OpenAIGPT
{
    [Serializable]
    public class GPTConversationPoint
    {
        public string name;
        [TextAreaAttribute]
        public string context;
        public bool sendsContext;
        public bool sendsResponse;
        public UnityEvent<string> action;
        public UnityEvent altAction;
    }
}