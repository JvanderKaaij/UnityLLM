using System.Collections.Generic;
using MLAgents;

public struct TrainingSummary
{
    public string contextSummary;
    public string previousCode;
    public TensorDataList previousTensorData;
    public HyperParameterConfig previousHyperParams;
}
