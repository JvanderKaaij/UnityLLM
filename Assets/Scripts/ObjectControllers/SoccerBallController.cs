using System;
using UnityEngine;

[GPTExpose]
public class SoccerBallController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;

    private Action OnDeath;
    private Action OnGoal;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    [GPTExpose]
    public void AddForce(Vector3 force)
    {
        rigidBody.AddForce(new Vector3(Mathf.Clamp(force.x, -1.0f, 1.0f), 0, Mathf.Clamp(force.z, -1.0f, 1.0f)), ForceMode.Force);
    }
    
    [GPTExpose]
    public void SubscribeToDeathEvent(Action response)
    {
        OnDeath = null;
        OnDeath += response;
    }
    
    [GPTExpose]
    public void SubscribeToOnGoalEvent(Action response)
    {
        OnGoal = null;
        OnGoal += response;
    }

    [GPTExpose]
    public void Reset()
    {
        transform.position = startPosition;
    }
    
    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.gameObject.name == "Goal")
        {
            Debug.Log("GOAL SCORED!!!");
            OnGoal?.Invoke();
        }
    }

    private void Update()
    {
        if (transform.position.y <= -4f)
        {
            Debug.Log($"DIED: {transform.position.y}");
            OnDeath?.Invoke();
        }
    }
    
}

