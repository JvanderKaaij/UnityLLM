using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArticulationBody))]
public class SpiderLegController:MonoBehaviour
{
    [SerializeField] int windowSize;
    Queue<float> actionHistory;
    float sum;
    ArticulationBody body;
    
    Vector3 initialPosition;
    Quaternion initialRotation;
    ArticulationDrive initialXDrive;

    private void Start()
    {
        actionHistory = new Queue<float>();
        body = GetComponent<ArticulationBody>();
        
        if (body != null)
        {
            // Store initial state
            initialPosition = body.transform.position;
            initialRotation = body.transform.rotation;
            initialXDrive = new ArticulationDrive();
            initialXDrive.targetVelocity = body.xDrive.targetVelocity;

            // Store other properties as needed
        }
        
    }

    public void Rotate(float force)
    {
        //Moving Average functionality
        if (actionHistory.Count >= windowSize)
        {
            sum -= actionHistory.Dequeue();
        }

        actionHistory.Enqueue(force);
        sum += force;

        float weightedForce = (sum / actionHistory.Count);

        var xDrive = body.xDrive;
        xDrive.targetVelocity = weightedForce;
        body.xDrive = xDrive;

    }

    public void Reset()
    {
        body.transform.position = initialPosition;
        body.transform.rotation = initialRotation;
        // body.jointAcceleration = new ArticulationReducedSpace(0f, 0f, 0f);
        body.jointPosition = new ArticulationReducedSpace(0f, 0f, 0f);
        body.jointForce = new ArticulationReducedSpace(0f, 0f, 0f);
        body.jointVelocity = new ArticulationReducedSpace(0f, 0f, 0f);
        
        var xDrive = body.xDrive;
        xDrive.targetVelocity = 0;
        body.xDrive = xDrive;
        
        actionHistory = new Queue<float>();
        sum = 0;
    }
    
}
