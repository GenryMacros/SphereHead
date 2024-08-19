using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverButton : MonoBehaviour
{
    public TMP_Text text;
    private bool _isSelected = false;
    private Timer _timer;
    
    
    void Start()
    {
        _timer = gameObject.AddComponent<Timer>();
        _timer.callback = FlickCarette;
        _timer.isLooping = true;
        _timer.waitTime =0.5f;
        _timer.isAffectedByGame = false;
    }
    
    public void OnPointerEnter(BaseEventData data)
    {   
        text.text = $"C:\\> {text.text}";
        _isSelected = true;
        _timer.Begin();
    }
    
    public void OnPointerExit(BaseEventData data)
    {
        UnHover();
        _isSelected = false;
        _timer.Stop();
        if (text.text[^1] == '_')
        {
            text.text = text.text.Remove(text.text.Length - 1);
        }
    }

    public void UnHover()
    {
        text.text = text.text.Replace("C:\\> ", "");
    }
    
    private void FlickCarette()
    {
        if (text.text[^1] == '_')
        {
            text.text = text.text.Remove(text.text.Length - 1);
        }
        else
        {
            text.text += "_";
        }
    }
}
