using System.Collections.Generic;
using UnityEngine;


public enum NotificationType
{
    PlayerBuff = 0,
    EnemyBuff = 1,
    Unlock = 2
}


public struct PendingNotification
{
    public string text;
    public NotificationType type;

    public PendingNotification(string initText, NotificationType tp)
    {
        text = initText;
        type = tp;
    }
}


public class Notificator : MonoBehaviour
{
    [SerializeField] 
    private float _notificationDissappearStartTime;
    [SerializeField] 
    private List<NotificationText> _textSockets;
    private List<PendingNotification> _pendingNotifications = new List<PendingNotification>();
    private Dictionary<NotificationType, Color> notificationType2Color = new Dictionary<NotificationType, Color>()
    {
        { NotificationType.PlayerBuff, new Color(102.0f / 255, 255.0f / 255, 88.0f / 255, 1.0f)},
        { NotificationType.EnemyBuff, new Color(203.0f / 255, 23.0f / 255, 40.0f / 255, 1.0f)},
        { NotificationType.Unlock, new Color(211.0f / 255, 196.0f / 255, 38.0f / 255, 1.0f)},
    };
    
    public void TextSocketOpened()
    {
        while (_pendingNotifications.Count > 0)
        {
            NotificationText availableSocket = FindAvailableSocket();
            if (!availableSocket)
            {
                break;
            }
            availableSocket.ResetText(_pendingNotifications[0].text, 
                                      notificationType2Color[_pendingNotifications[0].type]);
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

    public void AppendNotification(string text, NotificationType notificationType)
    {
        NotificationText availableSocket = FindAvailableSocket();
            
        if (!availableSocket)
        {
            _pendingNotifications.Add(new PendingNotification(text, notificationType));
        }
        else
        {
            availableSocket.ResetText(text, notificationType2Color[notificationType]);
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
