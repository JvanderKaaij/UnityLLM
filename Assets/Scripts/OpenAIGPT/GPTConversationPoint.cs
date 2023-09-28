using System;
using UnityEngine;
using UnityEngine.Events;

namespace OpenAIGPT
{
    [Serializable]
    public class GPTConversationPoint
    {
        public string name;
        [TextAreaAttribute (20,100)]
        public string context;
        public bool sendsContext;
        public bool sendsResponse;
        public bool addToSummary;
        public UnityEvent<string> action;
        public UnityEvent altAction;
    }
}