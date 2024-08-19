using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _test;
    [SerializeField] 
    private RawImage _image;
    [SerializeField] 
    private float _existTime;
    private bool _isActive = true;
    private float _timeAfterUnactive;

    public int index;
    public event Action<int> messageDestroyed;
    
    void Start()
    {
        Invoke(nameof(MakeUnactive), _existTime);
    }
    
    void Update()
    {
        if (GameController.instance.IsGamePaused())
        {
            return;
        }
        
        if (!_isActive)
        {
            _timeAfterUnactive += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1.0f, 0.0f, _timeAfterUnactive / 1.0f);
            _test.color = new Color(_test.color.r, _test.color. g, _test.color.b, newAlpha);
            _image.color = new Color(_image.color.r, _image.color. g, _image.color.b, newAlpha);
            if (newAlpha <= 0.0f)
            {
                messageDestroyed?.Invoke(index);
                Destroy(gameObject);
            }
        }
    }

    public void ChangeText(string newText)
    {
        _test.text = newText;
    }
    
    private void MakeUnactive()
    {
        _isActive = false;
    }
}
