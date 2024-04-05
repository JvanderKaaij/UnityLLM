using System;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] private Rigidbody rigid;
    
    private Action OnDeath;
    private Vector3 startPosition;


    private void Start()
    {
        startPosition = transform.position;
    }

    [GPTExpose]
    public void Reset()
    {
        transform.position = startPosition;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity  = Vector3.zero;
    }
    
    
    [GPTExpose]
    public void SubscribeToDeathEvent(Action response)
    {
        OnDeath = null;
        OnDeath += response;
    }
    
    private void Update()
    {
        if (transform.position.y <= -4f)
        {
            // Debug.Log($"DIED: {transform.position.y}");
            OnDeath?.Invoke();
        }
    }
}
