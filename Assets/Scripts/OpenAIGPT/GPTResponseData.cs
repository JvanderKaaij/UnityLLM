using System;
using System.Collections.Generic;

namespace OpenAIGPT
{
    [Serializable]
    public class Choice
    {
        public int index;
        public GPTResponseMessageData message;
        public GPTFinishDetails finish_details;
    }

    [Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }

    [Serializable]
    public class GPTResponseData
    {
        public string id;
        public string @object;
        public int created;
        public string model;
        public Choice[] choices;
        public Usage usage;
    }

    [Serializable]
    public class GPTResponseMessageData
    {
        public string role;
        public string content;

        public GPTMessageData ToGPTMessageData()
        {
            return new GPTMessageData()
            {
                role = role,
                content = new List<GPTMessageContentType> { new GPTMessageContentType(){type = "text", text = content} } //Text is fine for now as I will not expect any other type of answer back for now
            };
        }
    }

    [Serializable]
    public class GPTFinishDetails
    {
        public string type;
        public string stop;
    }
}