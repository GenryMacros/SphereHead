using System;
using TMPro;
using UnityEngine;


public class NotificationText : MonoBehaviour
{
    public float notificationDissappearTime;
    public string text;
    [SerializeField] 
    private TMP_Text _uiText;
    [SerializeField] 
    private Timer _timer;
    [SerializeField] 
    private Color _textColor;
    [SerializeField]
    private Notificator _notificatorHolder;
    private bool _isFading = false;
    
    
    public void ResetText(string newText)
    {
        _isFading = false;
        text = newText;
        _uiText.text = text;
        _uiText.color = _textColor;
        _timer.waitTime = notificationDissappearTime;
        _timer.Begin();
    }

    private void Start()
    {
        _timer.callback = StartFading;
    }

    void StartFading()
    {
        _isFading = true;
    }
    
    void Update()
    {
        if (_isFading)
        {
            Color newColor = _uiText.color;
            newColor.a -= Time.deltaTime;
            _uiText.color = newColor;
            if (_uiText.color.a <= 10)
            {
                text = null;
                _isFading = false;
            }
            _notificatorHolder.TextSocketOpened();
        }
    }
}
