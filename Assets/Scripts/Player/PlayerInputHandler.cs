using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private PlayerMove _playerMove;

    private void OnEnable()
    {
        _playerInput.actions.FindAction("Move").performed += HandleMove;
        _playerInput.actions.FindAction("Move").canceled += StopMove;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("Move").performed -= HandleMove;
        _playerInput.actions.FindAction("Move").canceled -= StopMove;
    }

    private void HandleMove(InputAction.CallbackContext ctx)
    {
        _playerMove.HandleMove(ctx.ReadValue<Vector2>());
    }

    private void StopMove(InputAction.CallbackContext ctx)
    {
        _playerMove.HandleMove(Vector2.zero);
    }
}
