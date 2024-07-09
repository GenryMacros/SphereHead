using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    [SerializeField] 
    protected TMP_Text scoreText;
    [SerializeField] 
    protected TMP_Text scoreMultiplierText;
    [SerializeField] 
    protected Image multiplierForeground;

    [SerializeField] 
    private float stayTime;
    [SerializeField] 
    private float fadeTime;
    [SerializeField] 
    private Timer timer;
    [SerializeField]
    private PlayerUpgrader _upgrader;
    
    private float _modifiedFadeTime;
    private float _modifiedStayTime;
    
    private float _fadingTime;
    private bool _isFading;
    private int _currentMultiplier = 1;
    private int _currentScore;
    
    
    void Start()
    {
        _modifiedStayTime = stayTime;
        
        multiplierForeground.fillAmount = 0;
        timer.isLooping = false;
        timer.waitTime = _modifiedStayTime;
        timer.callback = StartScoreFading;
        ChangeUiScore();
        ChangeUiMultiplier();
    }

    
    void Update()
    {
        if (_isFading)
        {
            _fadingTime = Math.Min(_fadingTime + Time.deltaTime, _modifiedFadeTime);
            multiplierForeground.fillAmount = Mathf.Lerp(1.0f, 0.0f, _fadingTime / _modifiedFadeTime);
            if (multiplierForeground.fillAmount <= 0)
            {
                _currentMultiplier = Math.Max(_currentMultiplier - 1, 1);
                if (_currentMultiplier != 1)
                {
                    multiplierForeground.fillAmount = 1.0f;
                    _fadingTime = 0.0f;
                }
                else
                {
                    multiplierForeground.fillAmount = 0.0f;
                    _isFading = false;
                }

                ChangeUiMultiplier();
            }
        }
    }

    void StartScoreFading()
    {
        _isFading = true;
        _fadingTime = 0.0f;
    }

    public void IncrementScore(int increment, bool isMultiplierGrown)
    {
        _isFading = false;
        multiplierForeground.fillAmount = 1.0f;
        
        try {
            _currentScore = checked (_currentScore + increment * _currentMultiplier);
            _upgrader.ScoreUpdate(_currentScore);
        } catch (OverflowException)
        {
            _currentScore = int.MaxValue;
        }

        if (increment > 0)
        {
            _currentMultiplier = isMultiplierGrown ? _currentMultiplier + 1 : _currentMultiplier;
        }

        _modifiedFadeTime = Mathf.Lerp(fadeTime, 0.5f, _currentMultiplier / 99.0f);
        _modifiedStayTime = Mathf.Lerp(stayTime, 0.5f, _currentMultiplier / 99.0f);;
        timer.waitTime = _modifiedStayTime;
        timer.Begin();
        
        ChangeUiScore();
        ChangeUiMultiplier();
    }
    
    void ChangeUiScore()
    {
        int digitsInScore = _currentScore.ToString().Length;
        int maxDigits = int.MaxValue.ToString().Length;
        string newScoreString = _currentScore.ToString();
        for (int i = 0; i < (maxDigits - digitsInScore); i++)
        {
            newScoreString = newScoreString.Insert(0,"0");
        }
        scoreText.text = newScoreString;
    }
    
    void ChangeUiMultiplier()
    {
        int digitsInMultiplier = _currentMultiplier.ToString().Length;
        int maxDigits = 2;
        
        string newMultiplierString = _currentMultiplier.ToString();
        for (int i = 0; i < (maxDigits - digitsInMultiplier); i++)
        {
            newMultiplierString = newMultiplierString.Insert(0,"0");
        }
        scoreMultiplierText.text = newMultiplierString;
    }
}
