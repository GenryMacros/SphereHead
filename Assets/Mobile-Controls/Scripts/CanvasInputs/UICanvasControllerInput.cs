using System.Collections.Generic;
using UnityEngine;

public class UICanvasControllerInput : MonoBehaviour
{

    [Header("Output")]
    public PlayerController inputs;
    public PlayerControllerEndScene inputs_end;
    
    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        if (GameController.instance.IsGamePaused())
        {
            return;
        }
        virtualMoveDirection.Normalize();
        inputs.Move(virtualMoveDirection);
    }
    
    public void VirtualLookInput(Vector2 virtualLookDirection)
    {
        if (GameController.instance.IsGamePaused())
        {
            return;
        }
        inputs_end.RotateCamPhone(virtualLookDirection);
    }
    
    public void VirtualShootInput(bool virtualShootState)
    {
        if (GameController.instance.IsGamePaused())
        {
            return;
        }
        inputs.isShooting = virtualShootState;
    }

    public void VirtualChangeInput(bool virtualQuickState)
    {
        if (virtualQuickState)
        {
            inputs.QuickWeaponChange();
        }
    }
    
    public void VirtualPauseInput(bool virtualQuickState)
    {
        if (virtualQuickState)
        {
            GameController.instance.Pause();
        }
    }
}