using UnityEngine;

public class MoonlanderFeet : MonoBehaviour
{

    [SerializeField] private MoonlanderController moonlander;
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Landed");
    }
}
