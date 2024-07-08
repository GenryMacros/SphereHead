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
    private int nextLetterToPrint = 0;
    private Timer _caretteTimer;
    
    public void ResetText(string newText, Color textColor)
    {
        notificationDissappearTimeRecorded = 0;
        _isFading = false;
        text = newText;
        _uiText.text = "_";
        _uiText.color = textColor;
        nextLetterToPrint = 0;
        Invoke(nameof(PrintLetter), 0.1f);
        _caretteTimer.Begin();
    }

    private void Start()
    {
        text = "";
        _caretteTimer = gameObject.AddComponent<Timer>();
        _caretteTimer.waitTime = 0.1f;
        _caretteTimer.isLooping = true;
        _caretteTimer.callback = PrintCarette;
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

    private void PrintLetter()
    {
        _uiText.text = _uiText.text.Replace("_", "");
        _uiText.text += text[nextLetterToPrint];
        if (nextLetterToPrint + 1 >= text.Length)
        {
            Invoke(nameof(StartFading), notificationDissappearStartTime);
            _caretteTimer.Stop();
        }
        else
        {
            nextLetterToPrint += 1;
            Invoke(nameof(PrintLetter), 0.1f);
        }
    }

    private void PrintCarette()
    {
        if (_uiText.text.Contains("_"))
        {
            _uiText.text = _uiText.text.Replace("_", "");   
        }
        else
        {
            _uiText.text += "_";   
        }
    }
}
