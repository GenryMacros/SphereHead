using TMPro;
using UnityEngine;


public class NotificationText : MonoBehaviour
{
    public float notificationDissappearStartTime;
    public string text = "";
    [SerializeField] 
    private TMP_Text _uiText;
    [SerializeField] 
    private Color _textColor;
    [SerializeField]
    private Notificator _notificatorHolder;
    private bool _isFading = false;
    private float notificationDissappearTime = 2;
    private float notificationDissappearTimeRecorded;
    
    public void ResetText(string newText)
    {
        notificationDissappearTimeRecorded = 0;
        _isFading = false;
        text = newText;
        _uiText.text = text;
        _uiText.color = _textColor;
        Invoke(nameof(StartFading), notificationDissappearStartTime);
    }

    private void Start()
    {
        text = "";
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
            notificationDissappearTimeRecorded += Time.deltaTime;
            newColor.a = Mathf.Lerp(1.0f, 0.0f, notificationDissappearTimeRecorded / notificationDissappearTime);
            _uiText.color = newColor;
            if (_uiText.color.a == 0)
            {
                text = "";
                _isFading = false;
            }
            _notificatorHolder.TextSocketOpened();
        }
    }
}
