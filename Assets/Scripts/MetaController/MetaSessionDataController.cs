using System;
using System.IO;
using UnityEngine;

public static class MetaSessionDataController
{
    public static MetaSessionData RetrieveSessionData(string filePath)
    {
        if (!File.Exists(filePath)) {
            CreateDefaultJsonFile(filePath);
        }
        
        string json = File.ReadAllText(filePath);
        MetaSessionData result = JsonUtility.FromJson<MetaSessionData>(json);
        return result;
    }
    
    static void CreateDefaultJsonFile(string filePath)
    {
        MetaSessionData defaultFile = new MetaSessionData();
        Save(filePath, defaultFile);
    }

    public static void Save(string filePath, MetaSessionData data)
    {
        data.date = DateTime.Now.ToString();
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }
}
