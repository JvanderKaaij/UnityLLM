namespace OpenAIGPT
{
    [System.Serializable]
    public class Choice
    {
        public int index;
        public GPTMessageData message;
        public string finish_reason;
    }

    [System.Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }

    [System.Serializable]
    public class GPTResponseData
    {
        public string id;
        public string @object;
        public int created;
        public string model;
        public Choice[] choices;
        public Usage usage;
    }
}