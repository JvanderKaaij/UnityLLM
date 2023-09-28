using System.IO;
using MLAgents;
using UnityEngine;
using YamlDotNet.Serialization;

public static class HyperParameterBridge
{
    public static HyperParameterConfig Read(string filePath)
    {
        var deserializer = new DeserializerBuilder().Build();
        using(TextReader text_reader = File.OpenText(filePath))
        {
            var hyperParams = deserializer.Deserialize<HyperParameterConfig>(text_reader);
            return hyperParams;
        }
    }
    
    public static void Write(HyperParameterConfig config, string filePath)
    {
        var serializer = new SerializerBuilder().Build();
        string newYaml = serializer.Serialize(config);

        string outputPath = Path.Combine(Application.streamingAssetsPath, filePath);
        File.WriteAllText(outputPath, newYaml);
        
        Debug.Log("Serialized data saved to: " + outputPath);
    }
    
}
