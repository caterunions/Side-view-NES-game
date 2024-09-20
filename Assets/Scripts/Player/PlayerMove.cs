using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _moveSpeed = 5f;

    private Vector2 _lastMoveInput;

    public void HandleMove(Vector2 moveInfo)
    {
        _lastMoveInput = moveInfo;
    }

    private void Update()
    {
        _rb.velocity = _lastMoveInput * _moveSpeed;
    }

    private void OnDisable()
    {
        _lastMoveInput = Vector2.zero;
        _rb.velocity = Vector2.zero;
    }
}
