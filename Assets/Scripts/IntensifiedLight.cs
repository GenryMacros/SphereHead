using System;
using UnityEngine;


public class IntensifiedLight : MonoBehaviour
{
    [SerializeField] 
    private float _timeStart;

    [SerializeField] 
    private float _intensifyTime;
    [SerializeField] 
    private float _endIntensity;
    [SerializeField] 
    private float _endArea;
    
    private bool _isStarted = false;
    private float _timePassedSinceOpen;
    private Light _light;
    private float _startArea;
    private float _startIntensity;
    
    public Action activateCallback;
    
    void Start()
    {
        Invoke(nameof(StartIntense), _timeStart);
        _light = gameObject.GetComponent<Light>();
        _startArea = _light.range;
        _startIntensity = _light.intensity;
    }
    
    void Update()
    {
        if (_isStarted && _timePassedSinceOpen < _intensifyTime)
        {
            _timePassedSinceOpen += Time.deltaTime;
            float t = _timePassedSinceOpen / _intensifyTime;
            _light.range = Mathf.Lerp(_startArea, _endArea, t);
            _light.intensity = Mathf.Lerp(_startIntensity, _endIntensity, t);

            if (_timePassedSinceOpen >= _intensifyTime)
            {
                activateCallback?.Invoke();
            }
        }
    }
    
    private void StartIntense()
    {
        _isStarted = true;
    }
}
