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
    private float multiplierStayTime;
    [SerializeField] 
    private float multiplierFadeTime;
    [SerializeField] 
    private Timer timer;

    private bool _isFading;
    private int _currentMultiplier= 1;
    private int _currentScore;
    
    void Start()
    {
        multiplierForeground.fillAmount = 0;
        timer.isLooping = false;
        timer.waitTime = multiplierStayTime;
        timer.callback = StartScoreFading;
        ChangeUiScore();
        ChangeUiMultiplier();
    }

    
    void Update()
    {
        if (_isFading)
        {
            multiplierForeground.fillAmount -= Time.deltaTime * _currentMultiplier;
            if (multiplierForeground.fillAmount <= 0)
            {
                _currentMultiplier -= 1;
                if (_currentMultiplier != 1)
                {
                    multiplierForeground.fillAmount = 1.0f;
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
    }

    public void IncrementScore(int increment, bool isMultiplierGrown)
    {
        _isFading = false;
        multiplierForeground.fillAmount = 1.0f;
        timer.Begin();
        
        try {
            _currentScore = checked (_currentScore + increment * _currentMultiplier);
        } catch (OverflowException)
        {
            _currentScore = int.MaxValue;
        }

        _currentMultiplier = isMultiplierGrown ? _currentMultiplier + 1 : _currentMultiplier;
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
