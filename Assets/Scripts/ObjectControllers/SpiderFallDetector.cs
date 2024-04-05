using UnityEngine;

public class SpiderFallDetector:MonoBehaviour
{
    [SerializeField] private SpiderController spider;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I DIED");
        spider.Die();
    }
}
