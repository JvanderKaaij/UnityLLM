using System;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using UnityEngine;

namespace DefaultNamespace.MLAgents
{
    public class MLCompilerReaction:MonoBehaviour
    {
        [SerializeField] private string behaviourName;
        [SerializeField] private int decisionParams;
        [SerializeField] private int maxSteps;
        [SerializeField] private int vectorObservations;
        [SerializeField] private int actions;

        //Retrieve actions and observations amount from the code
        public void OnCodeExtracted(string code, string className)
        {
            Debug.Log($"EXTRACTED CODE: {code}");
            actions = CountOccurrences(code, "actionBuffers.ContinuousActions");
            vectorObservations = CountOccurrences(code, "sensor.AddObservation");//TODO What type of Observation?
        }
        
        public static int CountOccurrences(string source, string pattern)
        {
            int count = 0, startIndex = 0;
        
            while ((startIndex = source.IndexOf(pattern, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                startIndex += pattern.Length; 
            }

            return count;
        }
        
        public void DoneCompiling(GameObject obj)
        {
            var decision = obj.AddComponent<DecisionRequester>();
            decision.DecisionPeriod = decisionParams;
            
            var agent = obj.GetComponent<Agent>();
            agent.MaxStep = maxSteps;

            var behaviour = obj.AddComponent<BehaviorParameters>();
            behaviour.BehaviorName = behaviourName;
            behaviour.BrainParameters.VectorObservationSize = vectorObservations * 3;//To support Vector3's this should actually be dependant on the type of Observation
            
            var behaveActionSpecs = behaviour.BrainParameters.ActionSpec;
            behaveActionSpecs.NumContinuousActions = actions;
            behaviour.BrainParameters.ActionSpec = behaveActionSpecs;

            behaviour.InferenceDevice = InferenceDevice.GPU;
            
            obj.SetActive(true);
        }
        
    }
}