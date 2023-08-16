using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMNT;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField] private string host;
    [SerializeField] private int port;
    [SerializeField] private bool talk;
    [SerializeField] private LMNTSpeech speech;
    private List<string> history = new();
    
    
    [ContextMenu("Test")]
    public void Test()
    {
        Post("What is cheese?");
    }
    
    //a function that returns the history of the conversation
    public string GetHistory()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var line in history)
        {
            builder.Append($"${line} ");
        }
        return builder.ToString();
    }
    
    //Make a Post request to the server
    public void Post(string prompt)
    {
        history.Add($"User: {prompt}");
        StartCoroutine(PostRequest(FormatInContext(prompt)));
    }
    private string FormatInContext(string prompt)
    {
        return prompt; //$"You are a chatbot that only answers A or B.";
        // $"You are an AI assistant. You will be given a question that you must answer. You must generate a very short answer and put that answer between <response> </response> tags. The question is: {prompt}";
        // $"This is a conversation between a user and a chatbot. Previously this has been said: {GetHistory()}. The user now says: {prompt}. The chatbot must respond in a correct and formal way. Put that response between <response> </response> tags. Put that response after this sentence.";
    }
    //PostRequest Coroutine
    private IEnumerator PostRequest(string prompt)
    {
        //Create a new WWWForm
        WWWForm form = new WWWForm();
        //Add the json to the form
        form.AddField("prompt", prompt);
        //Create a new WWW object with the host, port and path
        WWW www = new WWW(host + ":" + port + "/infer", form);
        //Wait for the request to finish
        yield return www;
        //Check if there is an error
        if (www.error == null)
        {
            //Print the response
            Debug.Log(www.text);
            
            history.Add($"Chatbot: {FilterResponse(www.text)}");
            Debug.Log(FilterResponse(www.text));
            
            if(talk){
                StartCoroutine(speech.Talk(FilterResponse(www.text)));
            }
        }
        else
        {
            //Print the error
            Debug.Log(www.error);
        }
    }
    //a function that filters the text out from inbetween the last <response> tags
    private string FilterResponse(string text)
    {
        //Find the index of the last <response> tag
        int index = text.LastIndexOf("<response>", StringComparison.Ordinal);
        //Return the text from the index of the last <response> tag to the end of the text
        var txt = text.Substring(index + 10);
        //remove everything after the </response> tag
        index = txt.IndexOf("</response>", StringComparison.Ordinal);
        return txt.Substring(0, index);
    }
    
}
