using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;
    
    [SerializeField]
    private GameObject _topLeftCorner;
    [SerializeField]
    private GameObject _bottomRightCorner;
    
    private Vector3 _initialShift;
    private Camera _cam;
    
    void Start()
    {
        _cam = GetComponent<Camera>();
        _initialShift = transform.position;
    }
    
    void FixedUpdate()
    {
        Vector2 playerViewportPos = _cam.WorldToViewportPoint(_player.transform.position);
        if (playerViewportPos.y > 0.7 || playerViewportPos.y < 0.7 || 
            playerViewportPos.x > 0.7 || playerViewportPos.x < 0.7)
        {
            float newX = _player.transform.position.x;
            float newZ = _player.transform.position.z;
            
            if (newX <= _topLeftCorner.transform.position.x)
            {
                newX = _topLeftCorner.transform.position.x;
            } else if (newX >= _bottomRightCorner.transform.position.x)
            {
                newX = _bottomRightCorner.transform.position.x; 
            }
            
            if (newZ >= _topLeftCorner.transform.position.z)
            {
                newZ = _topLeftCorner.transform.position.z;
            } else if (newZ <= _bottomRightCorner.transform.position.z)
            {
                newZ = _bottomRightCorner.transform.position.z;
            }
            
            _cam.transform.position = new Vector3(newX + _initialShift.x, _cam.transform.position.y, newZ + _initialShift.z);
        }
    }
}
