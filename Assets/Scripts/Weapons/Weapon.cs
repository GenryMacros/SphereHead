using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float rateOfFire;
    public float damage;
    public float knockbackPower;
    public int ammo;
    public float bulletSpeed;
    
    protected bool isReadyToFire = false;
    
    [SerializeField]
    protected Bullet bulletPrefab;
    [SerializeField]
    protected GameObject spawnPoint;
    
    protected void Start()
    {
        InvokeRepeating(nameof(MakeReadyToFire), 0, rateOfFire);
    }

    public virtual void Fire()
    {
        if (isReadyToFire)
        {
            isReadyToFire = false;
        }
    }
    
    void MakeReadyToFire()
    {
        isReadyToFire = true;
    }
}
