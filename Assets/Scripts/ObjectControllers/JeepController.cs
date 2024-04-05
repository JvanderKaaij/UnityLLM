using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JeepController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private InputActionReference left;
    [SerializeField] private InputActionReference right;
    [SerializeField] private Vector2 clampForce;
    
    private Action OnDeath;
    private Action OnGoal;

    private Vector3 startPosition;

    private bool leftDown;
    private bool rightDown;
    
    private void Start()
    {
        startPosition = transform.position;
        
        left.action.started += (x) => leftDown = true;
        left.action.canceled += (x) => leftDown = false;
        left.action.Enable();

        right.action.started += (x) => rightDown = true;
        right.action.canceled += (x) => rightDown = false;
        right.action.Enable();
    }

    [GPTExpose]
    public void AddEngineForce(float force)//comes in normalized
    {
        force *= clampForce.y;
        rigidBody.AddForce(new Vector3(Mathf.Clamp(force, clampForce.x, clampForce.y), 0, 0), ForceMode.Force);
        Debug.Log($"Force Applied: {force}");
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
        if (collisionInfo.gameObject.gameObject.name == "Star")
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

        if (leftDown) AddEngineForce(clampForce.x);
        if (rightDown) AddEngineForce(clampForce.y);
    }
}
