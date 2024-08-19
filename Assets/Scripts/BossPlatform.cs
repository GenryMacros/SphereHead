using UnityEngine;


public class BossPlatform : MonoBehaviour
{
    [SerializeField] 
    private Material _bossPlatformeMaterial;

    [SerializeField] 
    private float _timeToMaterialize;

    private float _timeSinceActivation;

    
    void Update()
    {
        if (_timeSinceActivation < _timeToMaterialize)
        {
            _timeSinceActivation += Time.deltaTime;
            float t = _timeSinceActivation / _timeToMaterialize;
        
            _bossPlatformeMaterial.SetFloat("alpha", Mathf.Lerp(0.0f, 1.0f, t));   
        }
    }
}
