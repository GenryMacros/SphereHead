using System.Collections.Generic;


[System.Serializable]
public struct UpgradeTree
{
    public List<Upgrade> upgrades;
}

[System.Serializable]
public struct WeaponUpgrade
{
    public float rateOfFireChangePercent;
    public float damageChangePercent;
    public float knockbackPowerChangePercent;
    public float bulletSpeedChangePercent;
    public float maxBulletTravelDistanceChangePercent;  
    public float spreadConeAngleChangePercent;
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
