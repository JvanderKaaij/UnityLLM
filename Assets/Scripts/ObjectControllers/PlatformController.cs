using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformController : MonoBehaviour
{
   public Vector2 randomRangeX;
   public Vector2 randomRangeZ;

   private Vector3 startPosition;
   
   private void Start()
   {
      startPosition = transform.position;
   }

   public void Reset()
   {
      transform.position = startPosition + new Vector3(
         Random.Range(randomRangeX.x, randomRangeX.y),
         0.0f,
         Random.Range(randomRangeZ.x, randomRangeZ.y)
         );
   }
}
