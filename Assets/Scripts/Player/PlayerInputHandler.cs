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

    [SerializeField]
    private PlayerAim _playerAim;

    [SerializeField]
    private PlayerFire _playerFire;

    [SerializeField]
    private Camera _camera;

    private void OnEnable()
    {
        if (_camera == null) _camera = Camera.main;

        _playerInput.actions.FindAction("Move").performed += HandleMove;
        _playerInput.actions.FindAction("Move").canceled += StopMove;

        _playerInput.actions.FindAction("Aim").performed += HandleAim;

        _playerInput.actions.FindAction("Light").performed += BeginLightFire;
        _playerInput.actions.FindAction("Light").canceled += StopLightFire;

        _playerInput.actions.FindAction("Heavy").performed += BeginHeavyFire;
        _playerInput.actions.FindAction("Heavy").canceled += StopHeavyFire;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("Move").performed -= HandleMove;
        _playerInput.actions.FindAction("Move").canceled -= StopMove;

        _playerInput.actions.FindAction("Aim").performed -= HandleAim;

        _playerInput.actions.FindAction("Light").performed -= BeginLightFire;
        _playerInput.actions.FindAction("Light").canceled -= StopLightFire;

        _playerInput.actions.FindAction("Heavy").performed -= BeginHeavyFire;
        _playerInput.actions.FindAction("Heavy").canceled -= StopHeavyFire;
    }

    private void HandleMove(InputAction.CallbackContext ctx)
    {
        _playerMove.HandleMove(ctx.ReadValue<Vector2>());
    }

    private void StopMove(InputAction.CallbackContext ctx)
    {
        _playerMove.HandleMove(Vector2.zero);
    }

    private void HandleAim(InputAction.CallbackContext ctx)
    {
        _playerAim.HandleAim(_camera.ScreenToWorldPoint(ctx.ReadValue<Vector2>()));
    }

    private void BeginLightFire(InputAction.CallbackContext ctx)
    {
        _playerFire.StartFiringLight();
    }

    private void StopLightFire(InputAction.CallbackContext ctx)
    {
        _playerFire.StopFiringLight();
    }

    private void BeginHeavyFire(InputAction.CallbackContext ctx)
    {
        _playerFire.StartFiringHeavy();
    }

    private void StopHeavyFire(InputAction.CallbackContext ctx)
    {
        _playerFire.StopFiringHeavy();
    }
}
