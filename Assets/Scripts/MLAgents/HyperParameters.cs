using System.Collections.Generic;

namespace MLAgents
{
    public class Config
    {
        public object DefaultSettings { get; set; }
        public Dictionary<string, Behavior> Behaviors { get; set; }
        public EnvSettings EnvSettings { get; set; }
        public EngineSettings EngineSettings { get; set; }
        public object EnvironmentParameters { get; set; }
        public CheckpointSettings CheckpointSettings { get; set; }
        public TorchSettings TorchSettings { get; set; }
        public bool Debug { get; set; }
    }

    public class Behavior
    {
        public string TrainerType { get; set; }
        public Hyperparameters Hyperparameters { get; set; }
        public int CheckpointInterval { get; set; }
        public NetworkSettings NetworkSettings { get; set; }
        public Dictionary<string, RewardSignal> RewardSignals { get; set; }
        public object InitPath { get; set; }
        public int KeepCheckpoints { get; set; }
        public bool EvenCheckpoints { get; set; }
        public int MaxSteps { get; set; }
        public int TimeHorizon { get; set; }
        public int SummaryFreq { get; set; }
        public bool Threaded { get; set; }
        public object SelfPlay { get; set; }
        public object BehavioralCloning { get; set; }
    }

    public class Hyperparameters
    {
        public int BatchSize { get; set; }
        public int BufferSize { get; set; }
        public double LearningRate { get; set; }
        public double Beta { get; set; }
        public double Epsilon { get; set; }
        public double Lambd { get; set; }
        public int NumEpoch { get; set; }
        public bool SharedCritic { get; set; }
        public string LearningRateSchedule { get; set; }
        public string BetaSchedule { get; set; }
        public string EpsilonSchedule { get; set; }
    }

    public class NetworkSettings
    {
        public bool Normalize { get; set; }
        public int HiddenUnits { get; set; }
        public int NumLayers { get; set; }
        public string VisEncodeType { get; set; }
        public object Memory { get; set; }
        public string GoalConditioningType { get; set; }
        public bool Deterministic { get; set; }
    }

    public class RewardSignal
    {
        public double Gamma { get; set; }
        public double Strength { get; set; }
        public NetworkSettings NetworkSettings { get; set; }
    }

    public class EnvSettings
    {
        public object EnvPath { get; set; }
        public object EnvArgs { get; set; }
        public int BasePort { get; set; }
        public int NumEnvs { get; set; }
        public int NumAreas { get; set; }
        public int Seed { get; set; }
        public int MaxLifetimeRestarts { get; set; }
        public int RestartsRateLimitN { get; set; }
        public int RestartsRateLimitPeriodS { get; set; }
    }

    public class EngineSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int QualityLevel { get; set; }
        public int TimeScale { get; set; }
        public int TargetFrameRate { get; set; }
        public int CaptureFrameRate { get; set; }
        public bool NoGraphics { get; set; }
    }

    public class CheckpointSettings
    {
        public string RunId { get; set; }
        public object InitializeFrom { get; set; }
        public bool LoadModel { get; set; }
        public bool Resume { get; set; }
        public bool Force { get; set; }
        public bool TrainModel { get; set; }
        public bool Inference { get; set; }
        public string ResultsDir { get; set; }
    }

    public class TorchSettings
    {
        public object Device { get; set; }
    }
}
