using System;
using System.Collections.Generic;

namespace OpenAIGPT
{
    [System.Serializable]
    public class GPTMessageWrapper
    {
        public float temperature;
        public string model;
        public int max_tokens;
        public GPTMessageData[] messages;
    }

    [Serializable]
    public class GPTMessageData
    {
        public string role;
        public List<GPTMessageContentType> content;
        //TODO Retrieve 
    }
    
    [Serializable]
    public class GPTMessageContentType
    {
        public string type;
        public string text;
        public GPTImageURL image_url;
    }
    
    [Serializable]
    public class GPTImageURL
    {
        public string url;
    }
    
}