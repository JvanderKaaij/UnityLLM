using System;
using DefaultNamespace;
using UnityEngine;


[GPTExpose]
public class BirdController : MonoBehaviour
{
    private Rigidbody rigid;
    
    public Action OnHitSpikes;

    private float beginTime;
    
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    
    //To Start the Physics
    private void FixedUpdate()
    {
        if (ConversationModerator.RUNNING)
        {
            if(beginTime == 0f) beginTime = Time.time;
            
            if (Time.time - beginTime >= 10)
            {
                rigid.isKinematic = false;
            }
        }
    }


    [GPTExpose]
    public void AddForceVertical(float force)
    {
        rigid.AddForce(Vector3.up * force);
    }

    [GPTExpose]
    public void SubscribeToDiedAction(Action method)
    {
        OnHitSpikes += method;
    }
    
    public void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Hit Spikes");
        OnHitSpikes?.Invoke();
    }

}
