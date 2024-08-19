using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private List<HoverButton> _buttons;


    public void Hide()
    {
        gameObject.SetActive(false);
        foreach (var button in _buttons)
        {
            button.UnHover();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

}
