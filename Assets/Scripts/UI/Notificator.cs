using System.Collections.Generic;
using UnityEngine;

public class Notificator : MonoBehaviour
{
    [SerializeField] 
    private float _notificationDissappearTime;
    [SerializeField] 
    private List<NotificationText> _textSockets;
    private List<string> _pendingNotifications;

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
            tex.notificationDissappearTime = _notificationDissappearTime;
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
            if (nt_text.text == null)
            {
                return nt_text;
            }
        }

        return null;
    }
    
}
