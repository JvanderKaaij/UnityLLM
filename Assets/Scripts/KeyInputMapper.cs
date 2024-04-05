using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KeyInputMapper : MonoBehaviour
{
    [SerializeField] private SpiderController spider;
    [SerializeField] private InputActionReference vertical;
    private float verticalValue;
    
    private void OnEnable()
    {
        // Ensure the input action is enabled
        vertical.action.Enable();

        // Subscribe to the 'performed' event to listen for input
        vertical.action.performed += OnVerticalInput;
        // Optionally, subscribe to the 'canceled' event if you need to handle when the input stops
        vertical.action.canceled += OnVerticalInputCanceled;
    }
    
    private void OnDisable()
    {
        // Unsubscribe from the input action events when the object is disabled
        vertical.action.performed -= OnVerticalInput;
        vertical.action.canceled -= OnVerticalInputCanceled;

        // It's also a good practice to disable the action when it's not needed to conserve resources
        vertical.action.Disable();
    }

    private void Update()
    {
        if (verticalValue != 0.0f)
        {
            Debug.Log($"Vert Input {verticalValue}");
            spider.ExtendFrontLeftKnee(verticalValue);
        }
    }

    private void OnVerticalInput(InputAction.CallbackContext context)
    {
        verticalValue = context.ReadValue<float>();
        Debug.Log($"Vertical input: {verticalValue}");
    }
    
    private void OnVerticalInputCanceled(InputAction.CallbackContext context)
    {
        // Handle the input being canceled if necessary\
        verticalValue = 0.0f;
        Debug.Log("Vertical input canceled");
    }
}
