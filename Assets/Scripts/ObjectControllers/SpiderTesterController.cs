using UnityEngine;

public class SpiderTesterController : MonoBehaviour
{

    [SerializeField] private SpiderController spider;
    // Update is called once per frame

    private bool active = false;
    void Update()
    {
        if (!active) return;
        float a = Random.Range(-2f, 2f);
        spider.ExtendBackLeftShoulder(a);
        float b = Random.Range(-2f, 2f);
        spider.ExtendBackLeftKnee(b);
        float c = Random.Range(-2f, 2f);
        spider.ExtendBackRightShoulder(c);
        float d = Random.Range(-2f, 2f);
        spider.ExtendBackRightKnee(d);
        float e = Random.Range(-2f, 2f);
        spider.ExtendBackLeftShoulder(e);
        float f = Random.Range(-2f, 2f);
        spider.ExtendFrontLeftKnee(f);
    }

    [ContextMenu("Toggle Active")]
    public void ToggleActive()
    {
        active = !active;
    }

    [ContextMenu("Call Reset")]
    public void CallReset()
    {
        active = false;
        spider.Reset();
    }
}
