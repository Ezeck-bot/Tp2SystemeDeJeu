using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    public Action<Vector2> m_onChangeMovement;
    private Vector2 m_moveInput;

    public Action<bool> m_onChangeJump;

    public Action<bool> m_onChangeRunning;

    public Action<bool> m_onChangeInteract;

    public Action<bool> m_onChangeCrafting;

    //inventory

    public void SetDependencies(GameController gameController)
    {

    }
    public void FixedUpdate()
    {
        m_onChangeMovement?.Invoke(m_moveInput);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_onChangeJump?.Invoke(true);
        }
        else if (context.canceled)
        {
            m_onChangeJump?.Invoke(false);
        }
        
        
    }

    public void OnRunning(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_onChangeRunning?.Invoke(true);
        }
        else if (context.canceled)
        {
            m_onChangeRunning?.Invoke(false);
        }
    }

    public void OnInterract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_onChangeInteract?.Invoke(true);
        }
        else if (context.canceled)
        {
            m_onChangeInteract?.Invoke(false);
        }
    }

    public void OnCrafting(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_onChangeCrafting?.Invoke(true);
        }
        else if (context.canceled)
        {
            m_onChangeCrafting?.Invoke(false);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AudioManager.Instance.ToggleVolumeUI();
        }
    }
}
