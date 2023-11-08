using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonlanderFeet : MonoBehaviour
{

    [SerializeField] private MoonlanderController moonlander;
    
    private void OnCollisionEnter(Collision collision)
    {
        moonlander.FeetHitFloor();
    }
}
