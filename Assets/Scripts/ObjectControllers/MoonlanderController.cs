using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class MoonlanderController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float gravityScale;
    
    [SerializeField] private InputActionReference horizontal;
    [SerializeField] private InputActionReference vertical;
    [SerializeField] private InputActionReference thrust;
    
    [SerializeField] private float thrustScaling;
    
    private Vector3 gravity;

    private Action OnLanded;
    private Action OnCrash;

    private int feetHitCount;

    private float horizontalInput;
    private float verticalInput;
    private bool thrusting;

    private Vector3 forceUp;
    private Vector3 forceHorizontal;
    
    private void Start()
    {
        gravity = Physics.gravity * gravityScale;
        
        thrust.action.started += (x) => thrusting = true;
        thrust.action.canceled += (x) => thrusting = false;
        thrust.action.Enable();
        
        horizontal.action.started += (x) => horizontalInput = x.ReadValue<float>();
        horizontal.action.canceled += (x) => horizontalInput = 0;
        horizontal.action.Enable();

        vertical.action.started += (x) => verticalInput = x.ReadValue<float>();
        vertical.action.canceled += (x) => verticalInput = 0;
        vertical.action.Enable();
    }

    private void FixedUpdate()
    {
        //Apply custom gravity
        Vector3 customGravity = (Physics.gravity - gravity) * rigidBody.mass;
        rigidBody.AddForce(customGravity, ForceMode.Acceleration);
        
        if (rigidBody.position.y <= -5.0f || rigidBody.position.y >= 10.0f){
            Debug.Log($"CRASH {rigidBody.position.y}");
            OnCrash?.Invoke();
        }
        
        rigidBody.AddForce(forceUp + forceHorizontal);
        
        forceUp = Vector3.zero;
        forceHorizontal = Vector3.zero;
    }

    [GPTExpose]
    public void ThrustUp(float amount)
    {
        forceUp = Vector3.up * (amount * thrustScaling);
    }
    
    [GPTExpose]
    public void ThrustHorizontal(Vector2 force)
    {
        force *= thrustScaling;
        forceHorizontal = new Vector3(force.x, 0, force.y);
    }

    [GPTExpose]
    public void SubscribeToLandedEvent(Action response)
    {
        OnLanded = null;
        OnLanded += response;
    }
    
    [GPTExpose]
    public void SubscribeToCrashEvent(Action response)
    {
        OnCrash = null;
        OnCrash += response;
    }

    [GPTExpose]
    public void Reset()
    {
        feetHitCount = 0;
        forceUp = Vector3.zero;
        forceHorizontal = Vector3.zero;
        rigidBody.position = new Vector3(Random.Range(-4f, 4f), 7f, Random.Range(-4f, 4f));
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }

    public void Update()
    {
        if (thrusting)
        {
            ThrustUp(1.0f);
        }

        if (horizontalInput != 0 || verticalInput != 0)
        {
            ThrustHorizontal(new Vector2(horizontalInput, verticalInput));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnLanded?.Invoke();
    }
}
