using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponWheelController : MonoBehaviour
{
    public Vector2 normalizedMousePosition;
    public float currentAngle;
    public int selection;

    public WeaponWheelCenter center;
    public WeaponWheelItem[] pieParts;

    private int previusSelection;
    private WeaponWheelItem menuItemSc;
    private WeaponWheelItem previousMenuItemSc;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        normalizedMousePosition = new Vector2(Mouse.current.position.x.value - Screen.width / 2,
                                              Mouse.current.position.y.value - Screen.height / 2);
        currentAngle = Mathf.Atan2(normalizedMousePosition.y, normalizedMousePosition.x) * Mathf.Rad2Deg;

        currentAngle = (currentAngle + 360) % 360;
        selection = (int)(currentAngle / 72);
        if (selection != previusSelection)
        {
            pieParts[previusSelection].Deselect();
            previusSelection = selection;
            pieParts[selection].Select();
            string selectedWeaponName = pieParts[selection].representedWeapon.GetWeaponName();
            int selectedWeaponAmmo = pieParts[selection].representedWeapon.GetAmmoCount();
            center.ChangeText(selectedWeaponName, selectedWeaponAmmo);
        } 
    }

    public Weapon GetSelectedWeapon()
    {
        return pieParts[previusSelection].representedWeapon;
    }
}
