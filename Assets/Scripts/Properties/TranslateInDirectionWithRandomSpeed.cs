using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateInDirectionWithRandomSpeed : MonoBehaviour
{
    [SerializeField]
    private Vector2 _moveDirection = Vector2.zero;

    [SerializeField]
    private float _minSpeed;

    [SerializeField]
    private float _maxSpeed;

    private Vector2 _normalDir;

    private float _speed;

    private void OnEnable()
    {
        _normalDir = _moveDirection.normalized;
        _speed = Random.Range(_minSpeed, _maxSpeed);
    }

    private void Update()
    {
        transform.position = new Vector3(
            transform.position.x + (_normalDir.x * _speed * Time.deltaTime),
            transform.position.y + (_normalDir.y * _speed * Time.deltaTime),
            0
            );
    }
}
