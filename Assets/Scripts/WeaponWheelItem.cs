using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelItem : MonoBehaviour
{
    public Color hoverColor;
    public Color baseColor;
    public Image background;
    public Weapon representedWeapon;
    
    void Start()
    {
        background.color = baseColor;
    }

    public void Select()
    {
        background.color = hoverColor;
    }

    public void Deselect()
    {
        background.color = baseColor;
    }
    
    public void SwitchIcon()
    {
        background.color = baseColor;
    }
}
