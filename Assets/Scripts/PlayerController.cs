using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : LivingBeing
{
    public Weapon activeWeapon;
    
    [SerializeField]
    protected Animator _animtor;
    [SerializeField]
    protected WeaponWheelController weaponWheel;
    [SerializeField]
    protected PlayerBar bar;
    
    private Vector2 _lastInput;
    private bool _isMoving = false;
    private bool _isShooting = false;

    protected override void Start()
    {
        base.Start();
        weaponWheel.gameObject.SetActive(false);
        foreach (WeaponWheelItem item in weaponWheel.pieParts)
        {
            item.representedWeapon.gameObject.SetActive(false);
        }
        activeWeapon.gameObject.SetActive(true);
     
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        _isShooting = context.performed;
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
    
    public void OnWeaponWheel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weaponWheel.gameObject.SetActive(true);
        }
        else
        {
            Weapon selectedWeapon = weaponWheel.GetSelectedWeapon();
            Debug.Log(selectedWeapon.GetWeaponName());
            if (selectedWeapon.IsReady())
            {
                activeWeapon.gameObject.SetActive(false);
                activeWeapon = selectedWeapon;
                activeWeapon.gameObject.SetActive(true);
                bar.gunName.text = activeWeapon.GetWeaponName();

                int ammotCount = activeWeapon.GetAmmoCount();
                if (ammotCount != -1)
                {
                    bar.gunName.text = bar.gunName.text + ":" + ammotCount;
                }
            }
            weaponWheel.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        bool isKnockbacked = ProcessKnockback();
        if (!isKnockbacked && canMove)
        {
            if (_isMoving)
            {
                Quaternion preTransformBarRotation = bar.gameObject.transform.rotation;

                Vector2 velocity = speed * Time.deltaTime * _lastInput;
                transform.Translate(new Vector3(velocity.x, 0, velocity.y), Space.World);
                transform.forward = new Vector3(_lastInput.x, 0, _lastInput.y);
                
                bar.gameObject.transform.rotation = preTransformBarRotation;
            }

            if (_isShooting)
            {
                activeWeapon.Fire();
                int ammotCount = activeWeapon.GetAmmoCount();
                if (ammotCount != -1)
                {
                    bar.gunName.text = activeWeapon.GetWeaponName() + ":" + ammotCount;
                }
            }
        }
    }

    public override void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir)
    {
        base.TakeDamage(damage, knockbackPower, knockbackDir);
        Debug.Log("attacked");
        bar.Damage(damage);
        if (hp <= 0)
        {
            //TODO
        }
    }
}
