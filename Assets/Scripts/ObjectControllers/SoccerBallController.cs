using System;
using UnityEngine;

[GPTExpose]
public class SoccerBallController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;

    private Action OnDeath;
    private Action OnGoal;
    private Action OnHitWall;

    private Vector3 startPosition;

    private Vector3 m_force;

    private void Start()
    {
        startPosition = rigidBody.position;
    }

    [GPTExpose]
    public void AddForce(Vector3 force)
    {
        m_force = new Vector3(Mathf.Clamp(force.x, -1.0f, 1.0f), 0, Mathf.Clamp(force.z, -1.0f, 1.0f));
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
    public void SubscribeToOnHitWallEvent(Action response)
    {
        OnHitWall = null;
        OnHitWall += response;
    }

    [GPTExpose]
    public void Reset()
    {
        rigidBody.position = startPosition;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }
    
    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.name == "Goal")
        {
            Debug.Log("GOAL SCORED!!!");
            OnGoal?.Invoke();
        }
        
        if (collisionInfo.gameObject.name == "Maze")
        {
            OnHitWall?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        rigidBody.AddForce(m_force, ForceMode.Force);
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

