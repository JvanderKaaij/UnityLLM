using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class TensorDataItem
{
    public double wall_time;
    public int step;
    public string tag;
    public double simple_value;
}

[Serializable]
public class TensorDataList
{
    public TensorDataItem[] message;

    public bool HasData()
    {
        return message.Length > 0;
    }
    
    public override string ToString()
    {
        var rewards = OrderedRewards();
        var result = new StringBuilder();
        result.Append("Rewards over time: \n");
        foreach (var reward in rewards)
        {
            result.Append($", {reward.simple_value}");
        }
        result.Append("\nEpisode Lengths over time: \n");
        var lengths = OrderedEpisodeLengths();
        foreach (var length in lengths)
        {
            result.Append($", {length.simple_value}");
        }
        return result.ToString();
    }
    
    private List<TensorDataItem> OrderedRewards()
    {
        List<TensorDataItem> rewards = message
            .Where(t => t.tag == "Environment/Cumulative Reward")
            .OrderBy(t => t.step)
            .ToList();
        return rewards;
    }
    
    private List<TensorDataItem> OrderedEpisodeLengths()
    {
        List<TensorDataItem> episodeLengths = message
                .Where(t => t.tag == "Environment/Episode Length")
                .OrderBy(t => t.step)
                .ToList();
        return episodeLengths;
    }
}