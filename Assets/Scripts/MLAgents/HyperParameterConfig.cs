using System.Collections.Generic;
using System.Text;
using Unity.XR.CoreUtils;

namespace MLAgents
{
    public class HyperParameterConfig
    {
        public object default_settings { get; set; }
        public Dictionary<string, Behavior> behaviors { get; set; }
        public EnvSettings env_settings { get; set; }
        public EngineSettings engine_settings { get; set; }
        public object environment_parameters { get; set; }
        public CheckpointSettings checkpoint_settings { get; set; }
        public TorchSettings torch_settings { get; set; }
        public bool debug { get; set; }


        public Behavior FirstBehavior()
        {
            return behaviors.First().Value;
        }
        
        public override string ToString()
        {
            var hp = FirstBehavior().hyperparameters;
            var result = new StringBuilder();
            result.Append($"batch_size: {hp.batch_size}\n");
            result.Append($"buffer_size: {hp.buffer_size}\n");
            result.Append($"learning_rate: {hp.learning_rate}\n");
            result.Append($"beta: {hp.beta}\n");
            result.Append($"epsilon: {hp.epsilon}\n");
            result.Append($"lambd: {hp.lambd}\n");
            result.Append($"num_epoch: {hp.num_epoch}\n");
            result.Append($"shared_critic: {hp.shared_critic}\n");
            result.Append($"learning_rate_schedule: {hp.learning_rate_schedule}\n");
            result.Append($"beta_schedule: {hp.beta_schedule}\n");
            result.Append($"epsilon_schedule: {hp.epsilon_schedule}\n");
            return result.ToString();
        }
        
    }

    public class Behavior
    {
        public string trainer_type { get; set; }
        public Hyperparameters hyperparameters { get; set; }
        public int checkpoint_interval { get; set; }
        public NetworkSettings network_settings { get; set; }
        public Dictionary<string, RewardSignal> reward_signals { get; set; }
        public object init_path { get; set; }
        public int keep_checkpoints { get; set; }
        public bool even_checkpoints { get; set; }
        public int max_steps { get; set; }
        public int time_horizon { get; set; }
        public int summary_freq { get; set; }
        public bool threaded { get; set; }
        public object self_play { get; set; }
        public object behavioral_cloning { get; set; }
    }

    public class Hyperparameters
    {
        public int batch_size { get; set; }
        public int buffer_size { get; set; }
        public double learning_rate { get; set; }
        public double beta { get; set; }
        public double epsilon { get; set; }
        public double lambd { get; set; }
        public int num_epoch { get; set; }
        public bool shared_critic { get; set; }
        public string learning_rate_schedule { get; set; }
        public string beta_schedule { get; set; }
        public string epsilon_schedule { get; set; }
    }

    public class NetworkSettings
    {
        public bool normalize { get; set; }
        public int hidden_units { get; set; }
        public int num_layers { get; set; }
        public string vis_encode_type { get; set; }
        public object memory { get; set; }
        public string goal_conditioning_type { get; set; }
        public bool deterministic { get; set; }
    }

    public class RewardSignal
    {
        public double gamma { get; set; }
        public double strength { get; set; }
        public NetworkSettings network_settings { get; set; }
    }

    public class EnvSettings
    {
        public object env_path { get; set; }
        public object env_args { get; set; }
        public int base_port { get; set; }
        public int num_envs { get; set; }
        public int num_areas { get; set; }
        public int seed { get; set; }
        public int max_lifetime_restarts { get; set; }
        public int restarts_rate_limit_n { get; set; }
        public int restarts_rate_limit_period_s { get; set; }
    }

    public class EngineSettings
    {
        public int width { get; set; }
        public int height { get; set; }
        public int quality_level { get; set; }
        public int time_scale { get; set; }
        public int target_frame_rate { get; set; }
        public int capture_frame_rate { get; set; }
        public bool no_graphics { get; set; }
    }

    public class CheckpointSettings
    {
        public string run_id { get; set; }
        public object initialize_from { get; set; }
        public bool load_model { get; set; }
        public bool resume { get; set; }
        public bool force { get; set; }
        public bool train_model { get; set; }
        public bool inference { get; set; }
        public string results_dir { get; set; }
    }

    public class TorchSettings
    {
        public object device { get; set; }
    }
}
