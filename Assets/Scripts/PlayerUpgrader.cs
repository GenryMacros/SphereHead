using System.Collections.Generic;
using UnityEngine;


public class PlayerUpgrader : MonoBehaviour
{
    [SerializeField] 
    private List<Weapon> _weapons;
    [SerializeField]
    private Notificator _notificator;
    
    private UpgradeTree _tree;
    private int _lastScore;
    private int _nextUpgrade = 0;
    
    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("TextData/upgrade_tree.json");
        _tree = JsonUtility.FromJson<UpgradeTree>(jsonFile.text);
    }
    
    public void ScoreUpdate(int newScore)
    {
        if (_lastScore > newScore)
        {
            return;
        }

        _lastScore = newScore;
        while (_nextUpgrade < _tree.upgrades.Count && _tree.upgrades[_nextUpgrade].scoreRequired < _lastScore)
        {
            Upgrade upgrade = _tree.upgrades[_nextUpgrade];
            foreach (var weapon in _weapons)
            {
                if (weapon.GetWeaponName() == upgrade.weaponName)
                {
                    weapon.ApplyUpgrade(upgrade);
                    _notificator.AppendNotification("+ " + upgrade.upgradeName);
                }
            }
            _nextUpgrade += 1;
        }
    }
}
