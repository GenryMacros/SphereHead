using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    public SettingsMenu settings;
    public List<HoverButton> otherButtons;
    public HoverButton settingsButton;
    public TMP_Text title;
    public VideoPlayer backGround;
    
    void Start()
    {
        ToMain();
        backGround.url = System.IO.Path.Combine (Application.streamingAssetsPath,"vid0001-0120.mp4"); 
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
