using System;
using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using UnityEngine;

namespace DefaultNamespace.MLAgents
{
    public class MLCompilerReaction:MonoBehaviour
    {
        [SerializeField] private int decisionParams;
        [SerializeField] private int maxSteps;
        [SerializeField] private int vectorObservations;
        [SerializeField] private int continouousActions;
        [SerializeField] private int discreteActions;

        //Retrieve actions and observations amount from the code
        public void OnCodeExtracted(string behaviourName, string code, string className)
        {
            Debug.Log($"EXTRACTED CODE: {code}");
            continouousActions = CountOccurrences(code, "actionBuffers.ContinuousActions");
            discreteActions = CountOccurrences(code, "actionBuffers.DiscreteActions");
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
        
        public void DoneCompiling(string behaviourName, GameObject obj)
        {
            var decision = obj.AddComponent<DecisionRequester>();
            decision.DecisionPeriod = decisionParams;
            
            var agent = obj.GetComponent<Agent>();
            agent.MaxStep = maxSteps;
            
            obj.AddComponent<BehaviorParameters>();
            
            StartCoroutine(WaitFrameForStart(behaviourName, obj));
        }

        IEnumerator WaitFrameForStart(string behaviourName, GameObject obj)
        {
            yield return new WaitForEndOfFrame();
            
            var behaviour = obj.GetComponent<BehaviorParameters>();
            behaviour.BehaviorName = behaviourName;
            behaviour.BrainParameters.VectorObservationSize = vectorObservations * 3;//To support Vector3's this should actually be dependant on the type of Observation
            var behaveActionSpecs = behaviour.BrainParameters.ActionSpec;
            behaveActionSpecs.NumContinuousActions = continouousActions;
            behaveActionSpecs.BranchSizes = new int[discreteActions];
            for (int i = 0; i < discreteActions; i++)
            {
                behaveActionSpecs.BranchSizes[i] = 1;
            }
            behaviour.BrainParameters.ActionSpec = behaveActionSpecs;
            behaviour.InferenceDevice = InferenceDevice.Burst;
            
            try
            {
                //TODO: Not working the try catch!
                obj.SetActive(true);
            }
            catch(Exception e)
            {
                Debug.Log("Error at enabling Agent");
            }
        }

    }
}