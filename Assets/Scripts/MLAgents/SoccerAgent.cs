using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GolfBallAgent : Agent
{
    private Rigidbody golfBallRigidbody;
    private Transform checkerboardTransform;

    private void Awake()
    {
        golfBallRigidbody = GameObject.Find("Sphere").GetComponent<Rigidbody>();
        checkerboardTransform = GameObject.Find("PurpleSquare").transform;
    }

    public override void OnEpisodeBegin()
    {
        // Reset the golf ball's position and velocity
        golfBallRigidbody.velocity = Vector3.zero;
        golfBallRigidbody.angularVelocity = Vector3.zero;

        // Randomize the initial positions of the golf ball and checkerboard
        Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f));
        golfBallRigidbody.transform.position = randomPosition;
        checkerboardTransform.position = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe the position of the golf ball and checkerboard
        sensor.AddObservation(golfBallRigidbody.transform.position);
        sensor.AddObservation(checkerboardTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Normalize the actions between -1 and 1
        float horizontalForce = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
        float verticalForce = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);

        // Apply forces to the golf ball based on the actions received
        Vector3 force = new Vector3(horizontalForce, 0f, verticalForce);
        golfBallRigidbody.AddForce(force * 10f);

        float distanceToCheckerboard = Vector3.Distance(golfBallRigidbody.transform.position, checkerboardTransform.position);

        // Give a reward based on the distance to the checkerboard
        float reward = 1.0f / (1.0f + distanceToCheckerboard);
        SetReward(reward);

        // End the episode if the ball is close enough or too far away
        if (distanceToCheckerboard < 0.5f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if (distanceToCheckerboard > 10f)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }
}