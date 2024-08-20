using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public SettingsMenu settings;
    public List<HoverButton> otherButtons;
    public HoverButton settingsButton;
    public TMP_Text title;
    
    
    void Start()
    {
        ToMain();
    }
    
    public void ToSettings()
    {
        foreach (var button in otherButtons)
        {
            button.gameObject.SetActive(false);
            button.UnHover();
        }
        settings.gameObject.SetActive(true);
        title.gameObject.SetActive(false);
    }

    public void ToMain()
    {
        foreach (var button in otherButtons)
        {
            button.gameObject.SetActive(true);
        }
        settingsButton.UnHover();
        settings.gameObject.SetActive(false);
        title.gameObject.SetActive(true);
    }
}
