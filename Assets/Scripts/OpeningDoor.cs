using System;
using UnityEngine;


public class OpeningDoor : MonoBehaviour
{
    [SerializeField] 
    private float _timeBeforeOpenStart;
    [SerializeField] 
    private float _openTime;
    [SerializeField] 
    private float _openAngle = -90;
    
    public AudioSource _source;
    
    private bool _isOpening = false;
    private float _timePassedSinceOpen;
    
    public Action activateCallback;
    
    void Start()
    {
        Invoke(nameof(StartOpening), _timeBeforeOpenStart);
    }
    
    void Update()
    {
        if (_isOpening && _timePassedSinceOpen < _openTime)
        {
            _timePassedSinceOpen += Time.deltaTime;
            float newY = Mathf.Lerp(0.0f, _openAngle, _timePassedSinceOpen / _openTime);
            Vector3 currentRotation = gameObject.transform.eulerAngles;
            gameObject.transform.eulerAngles = new Vector3(currentRotation.x, newY, currentRotation.z);

            if (_timePassedSinceOpen >= _openTime)
            {
                activateCallback?.Invoke();
            }
        }
    }

    private void StartOpening()
    {
        _isOpening = true;
        _source.Play();
    }
}
