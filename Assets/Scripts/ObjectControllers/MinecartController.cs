using System;
using UnityEngine;

[GPTExpose]
public class MinecartController : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float applyForce = 1.0f;
    private float force = 0.0f;
    
    private void Update()
    {
        rigidBody.AddForce(new Vector3(force, 0.0f, 0.0f));
    }

    [GPTExpose]
    public void Push()
    {
        force = applyForce;
    }
    
    [GPTExpose]
    public void EngineOff()
    {
        force = 0.0f;
    }
    
    [GPTExpose]
    public void Reverse()
    {
        force = -applyForce;
    }
}
