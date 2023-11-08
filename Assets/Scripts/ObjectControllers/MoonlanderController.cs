using System;
using UnityEngine;

public class MoonlanderController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;

    private Action OnLanded;
    
    private Vector3 startPosition;
    private int feetHitCount;

    private void Start()
    {
        startPosition = transform.position;
    }
    
    [GPTExpose]
    public void ThrustUp(float amount)
    {
        rigidBody.AddRelativeForce(Vector3.one * amount);
    }
    
    [GPTExpose]
    public void ThrustHorizontal(float amount)
    {
        rigidBody.AddRelativeForce(Vector3.right * amount);
    }

    [GPTExpose]
    public void SubscribeToLandedEvent(Action response)
    {
        OnLanded = null;
        OnLanded += response;
    }

    [GPTExpose]
    public void Reset()
    {
        feetHitCount = 0;
        transform.position = startPosition;
    }

    public void FeetHitFloor()
    {
        feetHitCount++;
        if (feetHitCount >= 4)
        {
            OnLanded?.Invoke();
        }
    }
    
}
