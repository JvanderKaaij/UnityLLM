using System;

namespace OpenAIGPT
{
    [System.Serializable]
    public class GPTMessageWrapper
    {
        public float temperature;
        public string model;
        public GPTMessageData[] messages;
    }

    [Serializable]
    public class GPTMessageData
    {
        public string role;
        public string content;
    }
}