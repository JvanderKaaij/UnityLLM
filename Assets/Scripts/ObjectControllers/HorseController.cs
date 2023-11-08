using UnityEngine;


[GPTExpose]
public class HorseController : MonoBehaviour
{
    private Transform m_target;
    private float m_hunger = 1f;
    
    [GPTExpose]
    public void WalkToTarget(GameObject target)
    {
        m_target = target.transform;
    }

    [GPTExpose]
    public float GetHunger()
    {
        return m_hunger;
    }
    
    [GPTExpose]
    public void Eat(GameObject target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= 1.0f)
        {
            m_hunger = 1.0f;
            target.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 0.0f, Random.Range(-1.5f, 1.5f));
        }
    }

    private void Update()
    {
        if(m_target){
            transform.position = Vector3.Lerp(transform.position, m_target.position, 0.1f);
            m_hunger -= 0.01f;
        }
    }
}
