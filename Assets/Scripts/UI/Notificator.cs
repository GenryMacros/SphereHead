using System.Collections.Generic;
using UnityEngine;

public class Notificator : MonoBehaviour
{
    [SerializeField] 
    private float _notificationDissappearStartTime;
    [SerializeField] 
    private List<NotificationText> _textSockets;
    private List<string> _pendingNotifications = new List<string>();

    public void TextSocketOpened()
    {
        while (_pendingNotifications.Count > 0)
        {
            NotificationText availableSocket = FindAvailableSocket();
            if (!availableSocket)
            {
                break;
            }
            availableSocket.ResetText(_pendingNotifications[0]);
            _pendingNotifications.Remove(_pendingNotifications[0]);
        }
    }
    
    void Start()
    {
        foreach (NotificationText tex in _textSockets)
        {
            tex.notificationDissappearStartTime = _notificationDissappearStartTime;
        }
    }
    
    void Update()
    {
        
    }

    public void AppendNotification(string text)
    {
        NotificationText availableSocket = FindAvailableSocket();
            
        if (!availableSocket)
        {
            _pendingNotifications.Add(text);
        }
        else
        {
            availableSocket.ResetText(text);
        }
    }

    private NotificationText FindAvailableSocket()
    {
        foreach (NotificationText nt_text in _textSockets)
        {
            if (nt_text.text == "")
            {
                return nt_text;
            }
        }

        return null;
    }
    
}
