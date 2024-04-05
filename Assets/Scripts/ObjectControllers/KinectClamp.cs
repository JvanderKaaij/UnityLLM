using UnityEngine;
using System.Collections;
    
[AddComponentMenu("Kinect/Kinect Clamp")]
    
public class KinectClamp : MonoBehaviour {
    	
    [System.Serializable]
    public class BoneClamp{ //Class for bones with min and max XYZ rotation values
        public Transform bone;
        public float minX = 0;
        public float maxX = 360;
        public float minY = 0;
        public float maxY = 360;
        public float minZ = 0;
        public float maxZ = 360;
    }
    
    public BoneClamp[] boneClamps;
    private Vector3 newV3 = new Vector3(0f,0f,0f);
    
    // Use this for initialization
    void Start(){
    		
    }
    
    // Update is called once per frame
    void Update(){
        foreach(BoneClamp clamp in boneClamps){ 
            clamp.minX = Mathf.Clamp(clamp.minX,-180,180);	
            clamp.minY = Mathf.Clamp(clamp.minY,-180,180);	
            clamp.minZ = Mathf.Clamp(clamp.minZ,-180,180);	
            clamp.maxX = Mathf.Clamp(clamp.maxX,-180,180);	
            clamp.maxY = Mathf.Clamp(clamp.maxY,-180,180);	
            clamp.maxZ = Mathf.Clamp(clamp.maxZ,-180,180);	
        }
    }
    
    // We use LateUpdate to grab the rotation from the Transform after all Updates from
    // other scripts have occured
    void LateUpdate(){
        foreach(BoneClamp clamp in boneClamps){
            float rotationX = NormalizeAngle(clamp.bone.localEulerAngles.x);
            float rotationY = NormalizeAngle(clamp.bone.localEulerAngles.y);
            float rotationZ = NormalizeAngle(clamp.bone.localEulerAngles.z);
    			
            rotationX = Mathf.Clamp (rotationX, clamp.minX, clamp.maxX);
            rotationY = Mathf.Clamp (rotationY, clamp.minY, clamp.maxY);
            rotationZ = Mathf.Clamp (rotationZ, clamp.minZ, clamp.maxZ);
    			
            newV3.x = rotationX;
            newV3.y = rotationY;
            newV3.z = rotationZ;
    			
            clamp.bone.localEulerAngles = newV3;
        }
    }
    
     
    float NormalizeAngle(float angle)
    {
        // Normalize the angle to be within -180 to 180, making it easier to work with
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
    
    
}