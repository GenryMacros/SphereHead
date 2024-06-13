using TMPro;
using UnityEngine;

public class WeaponWheelCenter : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text ammo;

    
    public void ChangeText(string titleText, int ammoCount)
    {
        title.text = titleText;
        ammo.text = ammoCount == -1 ? "Infinite" : ammoCount.ToString();
    }
}
