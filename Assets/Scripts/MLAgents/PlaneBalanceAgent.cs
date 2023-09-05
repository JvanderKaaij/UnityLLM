using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlaneBalanceAgent : Agent
{
    public Transform ball;
    public Transform plane;
    private Rigidbody ballRigidbody;

    // Starting position for reset
    private Vector3 ballStartPosition;
    private Vector3 planeStartPosition;
    private Quaternion planeStartRotation;

    private void Start()
    {
        ball = GameObject.Find("Sphere").transform;
        plane = GameObject.Find("Plane").transform;
        ballRigidbody = ball.GetComponent<Rigidbody>();
        ballStartPosition = ball.position;
        planeStartPosition = plane.position;
        planeStartRotation = plane.rotation;
    }

    public override void OnEpisodeBegin()
    {
        // Reset ball and plane
        ball.position = ballStartPosition;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        
        plane.position = planeStartPosition;
        plane.rotation = planeStartRotation;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Relative position of ball to plane
        sensor.AddObservation(ball.position - plane.position);

        // Ball's velocity
        sensor.AddObservation(ballRigidbody.velocity);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        
        Debug.Log(actionBuffers.ContinuousActions[0]);
        
        float tiltAmountX = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
        float tiltAmountY = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);

        Vector3 currentRotation = plane.rotation.eulerAngles;
        plane.rotation = Quaternion.Euler(currentRotation.x + tiltAmountX, currentRotation.y, currentRotation.z + tiltAmountY);

        // Rewards
        if (Mathf.Abs(ball.position.x - plane.position.x) < 0.5f && Mathf.Abs(ball.position.z - plane.position.z) < 0.5f)
        {
            AddReward(0.1f); // Small reward for keeping the ball close to the center
        }
        else
        {
            AddReward(-0.1f); // Small penalty for the ball being far from the center
        }

        // Episode termination
        if (ball.position.y < plane.position.y - 1f)
        {
            AddReward(-1f); // Large penalty for letting the ball fall off
            EndEpisode();
        }
    }
    

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // For manual control or testing
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

}
