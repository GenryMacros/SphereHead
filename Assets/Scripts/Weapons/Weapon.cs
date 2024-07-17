using UnityEngine;


public class Weapon : MonoBehaviour
{
    public float rateOfFire;
    public float damage;
    public float knockbackPower;
    public OwnerEntity owner = OwnerEntity.Player;
    public bool isReadyToFire = true;
    
    [SerializeField]
    protected ParticleSystem _particles;
    [SerializeField]
    protected bool isReady = false;
    [SerializeField]
    protected string weaponName;
    [SerializeField]
    protected GameObject spawnPoint;
    
    protected virtual void Start()
    {
        if (_particles)
        {
            _particles.Stop();
        }
    }

    public virtual void Fire()
    {
        if (isReadyToFire)
        {
            isReadyToFire = false;
            if (_particles)
            {
                _particles.Play();
            }
            Invoke(nameof(MakeReadyToFire), rateOfFire);
        }
    }
    
    protected void MakeReadyToFire()
    {
        isReadyToFire = true;
        if (_particles)
        {
            _particles.Stop();
        }
    }
    
    public string GetWeaponName()
    {
        return weaponName;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _particles.Stop();
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
        _particles.Stop();
    }
    
    public bool IsReady()
    {
        return isReady;
    }
    
    public virtual void ApplyUpgrade(Upgrade upgrade)
    {
        
    }
    
    public virtual void ResetSpawnPoint(GameObject newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
