using System.Collections.Generic;


[System.Serializable]
public struct UpgradeTree
{
    public List<Upgrade> upgrades;
}

[System.Serializable]
public struct WeaponUpgrade
{
    public float rateOfFireChange;
    public float damageChange;
    public float knockbackPowerChange;
    public float bulletSpeedChange;
    public float maxBulletTravelDistanceChange;  
    public float spreadConeAngleChange;
    public int bulletsPerShotChange;
    public int maxAmmoIncrement;
}

[System.Serializable]
public struct Upgrade
{
    public int scoreRequired;
    public string weaponName;
    public string upgradeName;
    public NotificationType type;
    public List<WeaponUpgrade> upgradeParameters;
}
