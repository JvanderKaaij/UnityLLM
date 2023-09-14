using System;
using System.Collections.Generic;
using System.Linq;

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

    public List<TensorDataItem> OrderedRewards()
    {
        List<TensorDataItem> rewards = message
            .Where(t => t.tag == "Environment/Cumulative Reward")
            .OrderBy(t => t.step)
            .ToList();
        return rewards;
    }
    
    public List<TensorDataItem> OrderedEpisodeLengths()
    {
        List<TensorDataItem> episodeLengths = message
                .Where(t => t.tag == "Environment/Episode Length")
                .OrderBy(t => t.step)
                .ToList();
        return episodeLengths;
    }
}