using System;
using UnityEngine;


public class Timer: MonoBehaviour
{
    public Action callback;
    public float waitTime = 0;
    public bool isLooping = false;
    public bool isAffectedByGame = true;
    
    private float timePassed = 0;
    private bool _isStopped = true;
    
    private void Update()
    {
        if (_isStopped || (isAffectedByGame && GameController.instance.IsGamePaused()))
        {
            return;
        }
        
        timePassed += Time.deltaTime;
        if (timePassed >= waitTime)
        {
            callback();
            Reset();
            _isStopped = !isLooping;
        }
    }

    public void Begin()
    {
        Reset();
        _isStopped = false;
    }
    
    public void Stop()
    {
        _isStopped = true;
    }

    public bool IsStopped()
    {
        return _isStopped;
    }
    
    public void Reset()
    {
        timePassed = 0;
    }
}
