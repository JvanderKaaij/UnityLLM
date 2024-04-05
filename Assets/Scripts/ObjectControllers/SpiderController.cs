using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[GPTExpose]
public class SpiderController : MonoBehaviour
{
    [SerializeField] private Vector2 shoulderExtendForceRange;
    [SerializeField] private Vector2 kneeExtendForceRange;
    
    [Header ("Shoulders")]
    [SerializeField] SpiderLegController FrontLeftShoulderExtend;
    [SerializeField] SpiderLegController BackLeftShoulderExtend;
    [SerializeField] SpiderLegController FrontRightShoulderExtend;
    [SerializeField] SpiderLegController BackRightShoulderExtend;
    
    [Header ("Knees")]
    [SerializeField] SpiderLegController FrontLeftKnee;
    [SerializeField] SpiderLegController BackLeftKnee;
    [SerializeField] SpiderLegController FrontRightKnee;
    [SerializeField] SpiderLegController BackRightKnee;
    
    [Header ("Goal")]
    [SerializeField] Transform goalObject;
    
    private Action OnDeath;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private ArticulationBody rootart;
    
    private float ToRange(Vector2 minMax, float normVal)
    {
        float val = (normVal + 1f)/2f;//from -1/1 to 0/1
        return (minMax.y - minMax.x) * normVal;
    }

    private void Start()
    {
        rootart = GetComponent<ArticulationBody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        if (transform.position.y <= -5)
        {
            OnDeath?.Invoke();
        }
    }

    public void Reset()
    {
        FrontLeftShoulderExtend.Reset();
        BackLeftShoulderExtend.Reset();
        FrontRightShoulderExtend.Reset();
        BackRightShoulderExtend.Reset();
        FrontLeftKnee.Reset();
        BackLeftKnee.Reset();
        FrontRightKnee.Reset();
        BackRightKnee.Reset();
        StartCoroutine(AfterResetLimbs());
    }

    IEnumerator AfterResetLimbs()
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 Tpos = startPosition + Vector3.up * 0.1f; ;
        Quaternion Trot = startRotation;
        rootart.TeleportRoot(Tpos, Trot);
    }
    
    public void Die()
    {
        Debug.Log("DIE CALLED");
        OnDeath?.Invoke();
    }
    
    [GPTExpose]
    public void SubscribeToDeathEvent(Action response)
    {
        OnDeath = null;
        OnDeath += response;
    }
    
    [GPTExpose]
    public float GetDistanceFromGoal()
    {
        var magnitude = (transform.position - goalObject.position).magnitude;
        return Mathf.Abs(magnitude);
    }
    
    //Front Left
    [GPTExpose]
    public void ExtendFrontLeftShoulder(float normalizedRange)
    {
        float newAngle = ToRange(shoulderExtendForceRange, normalizedRange);
        FrontLeftShoulderExtend.Rotate(newAngle);
    }
 
    [GPTExpose]
    public void ExtendFrontLeftKnee(float normalizedRange)
    {
        float newAngle = ToRange(kneeExtendForceRange, normalizedRange);
        FrontLeftKnee.Rotate(newAngle);
    }
    
    //Back Left
    [GPTExpose]
    public void ExtendBackLeftShoulder(float normalizedRange)
    {
        float newAngle = ToRange(shoulderExtendForceRange, normalizedRange);
        BackLeftShoulderExtend.Rotate(newAngle);
    }
    
    [GPTExpose]
    public void ExtendBackLeftKnee(float normalizedRange)
    {
        float newAngle = ToRange(kneeExtendForceRange, normalizedRange);
        BackLeftKnee.Rotate(newAngle);
    }
    
    //Front Right
    [GPTExpose]
    public void ExtendFrontRightShoulder(float normalizedRange)
    {
        float newAngle = ToRange(shoulderExtendForceRange, normalizedRange);
        FrontRightShoulderExtend.Rotate(newAngle);
    }
  
    [GPTExpose]
    public void ExtendFrontRightKnee(float normalizedRange)
    {
        float newAngle = ToRange(kneeExtendForceRange, normalizedRange);
        FrontRightKnee.Rotate(newAngle);
    }
    
    //Back Right
    [GPTExpose]
    public void ExtendBackRightShoulder(float normalizedRange)
    {
        float newAngle = ToRange(shoulderExtendForceRange, normalizedRange);
        BackRightShoulderExtend.Rotate(newAngle);
    }
 
    [GPTExpose]
    public void ExtendBackRightKnee(float normalizedRange)
    {
        float newAngle = ToRange(kneeExtendForceRange, normalizedRange);
        BackRightKnee.Rotate(newAngle);
    }   
}
