using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float rateOfFire;
    public float damage;
    public float knockbackPower;
    public int ammo;
    public float bulletSpeed;
    
    protected bool isReadyToFire = true;
    
    [SerializeField]
    protected Bullet bulletPrefab;
    [SerializeField]
    protected GameObject spawnPoint;
    
    protected void Start()
    {
        
    }

    public virtual void Fire()
    {
        if (isReadyToFire)
        {
            isReadyToFire = false;
            Invoke(nameof(MakeReadyToFire), rateOfFire);
        }
    }
    
    void MakeReadyToFire()
    {
        isReadyToFire = true;
    }
}
