using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveMovement : BulletBehaviour
{
    [SerializeField]
    private float _frequency;

    [SerializeField]
    private float _magnitude;

    private float _timeAlive;

    private void Update()
    {
        _timeAlive += Time.deltaTime;
        transform.position = 
            new Vector2(transform.position.x, transform.position.y) + 
            new Vector2(transform.right.x, transform.right.y) * 
            Mathf.Cos(_timeAlive * _frequency) * (_magnitude * Time.deltaTime);
    }
}
