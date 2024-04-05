using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPosition : MonoBehaviour
{
   public Vector2 posRange;

   private float startPosX;
   public void Start()
   {
      startPosX = transform.position.x;
   }

   [GPTExpose]
   public void SetRandomPosition()
   {
      transform.position = new Vector3(startPosX + Random.Range(posRange.x, posRange.y), transform.position.y, transform.position.z);
   }
}
