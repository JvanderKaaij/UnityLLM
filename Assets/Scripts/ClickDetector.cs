using UnityEngine;
using UnityEngine.InputSystem;

public class ClickDetector : MonoBehaviour
{
    public InputActionReference click;
    public ConnectFourBoard board;
    
    private void Start()
    {
        click.action.performed += ctx => OnClick();
    }

    void OnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Debug.Log("CLICK");
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Clickable"))
            {
                int id = hit.collider.gameObject.GetComponent<ConnectFourInteractor>().SlotID;
                Debug.Log($"Make Move {id}");
                board.HumanMakeMove(id);
            }
        }
    }
}
