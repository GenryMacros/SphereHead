using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;

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
            _cam.transform.position = new Vector3(_player.transform.position.x + _initialShift.x, 
                                                  _cam.transform.position.y, 
                                                  _player.transform.position.z + _initialShift.z);
        }
    }
}
