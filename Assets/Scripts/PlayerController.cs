using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : LivingBeing
{
    public Weapon activeWeapon;
    
    private Vector2 _lastInput;
    private bool _isMoving = false;
    private bool _isShooting = false;

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isShooting = true;
        }
        else
        {
            _isShooting = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _lastInput = input;
        if (input == Vector2.zero)
        {
            _isMoving = false;
            return;
        }

        _isMoving = true;
    }

    private void Update()
    {
        if (_isShooting)
        {
            activeWeapon.Fire();
        }
    }

    void FixedUpdate()
    {
        if (_isMoving)
        {
            Vector2 velocity = speed * Time.deltaTime * _lastInput;
            transform.Translate(new Vector3(velocity.x, 0, velocity.y), Space.World);
            transform.forward = new Vector3(_lastInput.x, 0, _lastInput.y);
        }
    }

    public override void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir)
    {
        
    }
}
