using System;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _flickeringText;
    
    private Timer _flickerTimer;
    private bool _inTransition = false;
    private bool _isAcceptingInput = false;
    
    public void Activate()
    {
        if (!_flickerTimer)
        {
            _flickerTimer = gameObject.AddComponent<Timer>();
            _flickerTimer.waitTime = 0.5f;
            _flickerTimer.isLooping = true;
            _flickerTimer.callback = FlickerText;
        }
        gameObject.SetActive(true);
        _flickerTimer.Begin();
        Invoke(nameof(StartAcceptInput), 0.5f);
    }
    
    private void FlickerText()
    {
        CanvasRenderer rend = _flickeringText.GetComponent<CanvasRenderer>();
        if (rend.GetAlpha() >= 1.0f)
        {
            rend.SetAlpha(0.0F);
        }
        else
        {
            rend.SetAlpha(1.0F);
        }
    }

    private void Update()
    {
       if (Input.anyKey && !_inTransition && _isAcceptingInput)
       { 
           ScenesController.instance.ToMainMenu();
           _inTransition = true;
       }
    }

    private void StartAcceptInput()
    {
        _isAcceptingInput = true;
    }
}
