using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedSinWaveMovement : BulletBehaviour
{
    [SerializeField]
    private float _minFrequency;
    [SerializeField]
    private float _maxFrequency;

    private float _frequency;

    [SerializeField]
    private float _minMagnitude;
    [SerializeField]
    private float _maxMagnitude;

    private float _magnitude;

    private float _timeAlive;

    private void OnEnable()
    {
        _magnitude = Random.Range(_minMagnitude, _maxMagnitude);
        _frequency = Random.Range(_minFrequency, _maxFrequency);
    }

    private void Update()
    {
        _timeAlive += Time.deltaTime;
        transform.position = new Vector2(transform.position.x, transform.position.y) + new Vector2(transform.right.x, transform.right.y) * Mathf.Cos(_timeAlive * _frequency) * (_magnitude * 0.01f);
    }
}
