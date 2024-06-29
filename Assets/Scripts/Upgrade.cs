using System.Collections.Generic;


[System.Serializable]
public struct UpgradeTree
{
    public List<Upgrade> upgrades;
}


[System.Serializable]
public struct Upgrade
{
    public int scoreRequired;
    public string weaponName;
    public string upgradeName;
    public string upgradeParameters;
}
