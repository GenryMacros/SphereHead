using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DissappeatingTutor : MonoBehaviour
{
    [SerializeField] 
    private float _dissappearTime;
    [SerializeField] 
    private List<TMP_Text> _texts;
    
    private float _timePassed;
    

    void Update()
    {
        if (GameController.instance.IsGamePaused())
        {
            return;
        }
        
        if (_timePassed < _dissappearTime)
        {
            _timePassed += Time.deltaTime;
            float t = _timePassed / _dissappearTime;
            float newAlpha = Mathf.Lerp(1.0f, 0.0f, t);
            
            foreach (var text in _texts)
            {
                text.color = new Vector4(text.color.r, text.color.g, text.color.b, newAlpha);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
