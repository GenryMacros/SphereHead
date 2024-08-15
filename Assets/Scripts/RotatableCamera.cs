using System;
using UnityEngine;


public class RotatableCamera : MonoBehaviour
{
    private float _yaw;
    private float _pitch;

    
    public void UpdateRotation(Vector2 change)
    {
        _yaw += change.x;
        _pitch -= change.y;
        
        if (_pitch < -50.0f)
        {
            _pitch = -50.0f;
        }
        
        if (_pitch > 50.0f)
        {
            _pitch = 50.0f;
        } else if (_pitch < -230.0f)
        {
            _pitch = -230.0f;
        }
        transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
    }
}
