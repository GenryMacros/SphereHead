using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControllerEndScene : MonoBehaviour
{
    [SerializeField] 
    private float _camRotateSpeed = 1.0f;

    [SerializeField] 
    private RotatableCamera _cam;
    
    
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    
    public void OnRotate(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _cam.UpdateRotation(new Vector2(input.x, input.y) * _camRotateSpeed);
    }
    
}
