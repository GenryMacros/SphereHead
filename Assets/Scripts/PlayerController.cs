using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : LivingBeing
{
    public Weapon activeWeapon;
    
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected WeaponWheelController weaponWheel;
    [SerializeField]
    protected PlayerBar bar;

    private Weapon _newWeapon = null;
    private bool _isChangingWeapon = false;
    private Vector2 _lastInput;
    private bool _isMoving = false;
    private bool _isShooting = false;
    private bool _isReadyToShoot = true;
    
    protected override void Start()
    {
        base.Start();
        weaponWheel.gameObject.SetActive(false);
        foreach (WeaponWheelItem item in weaponWheel.pieParts)
        {
            item.representedWeapon.gameObject.SetActive(false);
        }
        activeWeapon.gameObject.SetActive(true);
        List<Weapon> weapons = weaponWheel.GetArsenal();

        foreach (Weapon weapon in weapons)
        {
            Gun cast = weapon as Gun;
            if (cast)
            {
                cast.ammoChanged += AmmoChange;
            }
        }
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        _isShooting =  context.performed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _lastInput = input;
        if (input == Vector2.zero)
        {
            _isMoving = false;
            _animator.SetBool("IsWalking", false);
            return;
        }

        _animator.SetBool("IsWalking", true);
        _isMoving = true;
    }
    
    public void OnWeaponWheel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weaponWheel.Activate();
            _animator.ResetTrigger("Rearm");
        }
        else
        {
            _isChangingWeapon = true;
            Weapon selectedWeapon = weaponWheel.GetSelectedWeapon();
            
            if (selectedWeapon.IsReady())
            {
                _newWeapon = selectedWeapon;
                _isReadyToShoot = false;
                _animator.SetTrigger("Rearm");
            }
            weaponWheel.Deactivate();
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

            if (_isReadyToShoot && _isShooting)
            {
                activeWeapon.Fire();
            }
        }

        if (_newWeapon && _isChangingWeapon && _animator.GetCurrentAnimatorStateInfo(1).IsName("Rearm") && _animator.IsInTransition(1))
        {
            activeWeapon.gameObject.SetActive(false);
            SwitchWeapon(_newWeapon);
            _newWeapon = null;
            _isChangingWeapon = false;
        } else if (!_isChangingWeapon && _animator.GetCurrentAnimatorStateInfo(1).IsName("Armature_Rearm_Back") && _animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1)
        {
            _isReadyToShoot = true;
        }
    }
    
    public override void TakeDamage(float damage, float knockbackPower, Vector2 knockbackDir, OwnerEntity damageCauser)
    {
        base.TakeDamage(damage, knockbackPower, knockbackDir, damageCauser);

        bar.Damage(damage);
        if (hp <= 0)
        {
            //TODO
        }
    }

    private void SwitchWeapon(Weapon newWeapon)
    {
        activeWeapon.Deactivate();
        activeWeapon = newWeapon;
        activeWeapon.Activate();
        bar.gunName.text = activeWeapon.GetWeaponName();

        AmmoChange();
    }

    private void AmmoChange()
    {
        Gun cast = activeWeapon as Gun;
        if (cast && !cast.IsInfiniteAmmo())
        {
            int ammoCount = cast.GetAmmoCount();
            bar.gunName.text = activeWeapon.GetWeaponName() + ":" + ammoCount;
        }
    }
    
    public float GetHP()
    {
        return bar.GetCurrentHealth();
    }

    public float GetMaxHP()
    {
        return bar.maxHealth;
    }

    public void Heal(float value)
    {
        bar.Heal(value);
    }

    public List<Weapon> GetArsenal()
    {
        return weaponWheel.GetArsenal();
    } 
}
