using UnityEngine;
using UnityEngine.InputSystem;

public class PlateController:MonoBehaviour
{
    [SerializeField] private InputActionReference horizontal;
    [SerializeField] private InputActionReference vertical;

    [SerializeField] private Rigidbody rigid;
    
    private Quaternion startRot;
    private float horizontalInput;
    private float verticalInput;
    
    public void Start()
    {
        startRot = transform.rotation;
        
        horizontal.action.started += (x) => horizontalInput = x.ReadValue<float>();
        horizontal.action.canceled += (x) => horizontalInput = 0;
        horizontal.action.Enable();

        vertical.action.started += (x) => verticalInput = x.ReadValue<float>();
        vertical.action.canceled += (x) => verticalInput = 0;
        vertical.action.Enable();
        
    }

    public void Control(Vector3 control)
    {
        Vector3 clampedVector = new Vector3(
            Mathf.Clamp(control.x, -1, 1),
            Mathf.Clamp(control.y, -1, 1),
            Mathf.Clamp(control.z, -1, 1)
        );
        rigid.AddTorque(new Vector3(clampedVector.x, clampedVector.y, clampedVector.z));
    }

    [GPTExpose]
    public void Reset()
    {
        transform.rotation = startRot;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity  = Vector3.zero;
    }
    
    private void Update()
    {
        if (verticalInput != 0)
        {
            rigid.AddTorque(new Vector3(verticalInput, 0, 0));
        }

        if (horizontalInput != 0)
        {
            rigid.AddTorque(new Vector3(0, 0, horizontalInput));
        }
    }
    
}
