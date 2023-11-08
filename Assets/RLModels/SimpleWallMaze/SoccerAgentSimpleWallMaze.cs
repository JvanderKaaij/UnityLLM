using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class SoccerAgentSimpleWallMaze : Agent
{
    private GameObject ball;
    private SoccerBallController ballController;
    private GameObject goal;
    
    private void Awake()
    {
        // Find the necessary game objects by name
        ball = GameObject.Find("Ball");
        goal = GameObject.Find("Goal");
        
        if (ball != null)
        {
            ballController = ball.GetComponent<SoccerBallController>();
            if (ballController != null)
            {
                // Subscribe to events
                ballController.SubscribeToDeathEvent(OnBallDeath);
                ballController.SubscribeToOnGoalEvent(OnGoalScored);
            }
        }
    }

    public override void OnEpisodeBegin()
    {
        if (ballController != null)
        {
            ballController.Reset();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (ball != null && goal != null)
        {
            // Ball's position and velocity
            sensor.AddObservation(ball.transform.position);
            sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity);
            
            // Distance and direction from the ball to the goal
            Vector3 toGoal = goal.transform.position - ball.transform.position;
            sensor.AddObservation(toGoal.normalized);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (ballController != null)
        {
            Vector3 force = new Vector3(actionBuffers.ContinuousActions[0], 
                                        actionBuffers.ContinuousActions[1], 
                                        actionBuffers.ContinuousActions[2]);
            
            ballController.AddForce(force);
        }
        
        // Reward based on ball's direction towards the goal
        Vector3 ballToGoal = goal.transform.position - ball.transform.position;
        float directionReward = Vector3.Dot(ball.GetComponent<Rigidbody>().velocity.normalized, ballToGoal.normalized);
        AddReward(0.005f * directionReward);
        
        // Small negative reward for each time step
        AddReward(-0.001f);
    }
    
    private void OnBallDeath()
    {
        AddReward(-1f);
        EndEpisode();
    }
    
    private void OnGoalScored()
    {
        AddReward(1f);
        EndEpisode();
    }
}