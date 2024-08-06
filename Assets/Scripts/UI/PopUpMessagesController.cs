using System.Collections.Generic;
using UnityEngine;


public class PopUpMessagesController : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> _messagePositions;

    [SerializeField] 
    private PopUp _popUpAsset;
    
    private string[] _possibleMessages;
    private Dictionary<int, bool> positionIndex2Availability = new Dictionary<int, bool>();
    
    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("TextData/screen_messages");
        _possibleMessages = jsonFile.text.Split('\n');
        for (int i = 0; i < _messagePositions.Count; i++)
        {
            positionIndex2Availability[i] = true;
        }
    }
    
    void Update()
    {
        
    }

    void PopUpDestroyed(int index)
    {
        positionIndex2Availability[index] = true;
    }
    
    public void SpawnMessage()
    {
        string randomText = _possibleMessages[Random.Range(0, _possibleMessages.Length - 1)];
        
        int posIndex = Random.Range(0, _messagePositions.Count - 1);
        if (!positionIndex2Availability[posIndex])
        {
            posIndex = Random.Range(0, _messagePositions.Count - 1);
        }
        positionIndex2Availability[posIndex] = false;
        
        GameObject popPos = _messagePositions[posIndex];
        
        PopUp newPopUp = Instantiate(_popUpAsset, popPos.transform);
        newPopUp.index = posIndex;
        newPopUp.messageDestroyed += PopUpDestroyed;
        
        newPopUp.ChangeText(randomText);
    }
    
}
