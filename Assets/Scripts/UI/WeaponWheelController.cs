using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponWheelController : MonoBehaviour
{
    public Vector2 normalizedMousePosition;
    public int selection;

    public WeaponWheelCenter center;
    public WeaponWheelItem[] pieParts;
    
    [SerializeField]
    protected TMP_Text _modulesInstalledText;

    private int _modulesInstalled = 0;
    private int _previousSelection;
    private WeaponWheelItem _menuItemSc;
    private WeaponWheelItem _previousMenuItemSc;
    
    void Update()
    {
        normalizedMousePosition = new Vector2(Mouse.current.position.x.value - Screen.width / 2.0f,
                                              Mouse.current.position.y.value - Screen.height / 2.0f);
        normalizedMousePosition.Normalize();
        
        float angle = Mathf.Atan2(normalizedMousePosition.y, normalizedMousePosition.x) / Mathf.PI;
        angle *= 180;
        angle += 90.0f;

        if (angle < 0)
        {
            angle += 360;
        }

        for (int i = 0; i < pieParts.Length; i++)
        {
            if (angle > i * 90 && angle < (i + 1) * 90)
            {
                selection = i;
                break;
            }
        }
       
        if (selection != _previousSelection)
        {
            pieParts[_previousSelection].Deselect();
            _previousSelection = selection;
            pieParts[selection].Select();
            string selectedWeaponName = pieParts[selection].representedWeapon.IsReady() ? pieParts[selection].representedWeapon.GetWeaponName() : "????";

            Gun cast = pieParts[selection].representedWeapon as Gun;
            int selectedWeaponAmmo = -1;
            if (cast)
            {
                selectedWeaponAmmo = cast.GetAmmoCount();
                cast.upgradeInstalled += UpgradeInstalled;
            }
            center.ChangeText(selectedWeaponName, selectedWeaponAmmo);
        } 
    }
    
    public Weapon GetSelectedWeapon()
    {
        return pieParts[_previousSelection].representedWeapon;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _modulesInstalledText.text = $"Modules installed: {_modulesInstalled} / 22";
        foreach (WeaponWheelItem part in pieParts)
        {
            part.DetermineBackground();
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public List<Weapon> GetActiveArsenal()
    {
        List<Weapon> weapons = new List<Weapon>();
        foreach (var piePart in pieParts)
        {
            if (piePart.representedWeapon.IsReady())
            {
                weapons.Add(piePart.representedWeapon);
            }
        }
        return weapons;
    }
    
    public List<Weapon> GetArsenal()
    {
        List<Weapon> weapons = new List<Weapon>();
        foreach (var piePart in pieParts)
        {
            weapons.Add(piePart.representedWeapon);
        }
        return weapons;
    }
    
    private void UpgradeInstalled()
    {
        _modulesInstalled += 1;
        _modulesInstalledText.text = $"Modules installed: {_modulesInstalled} / 22";
    }
}
