using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantForwardVelocity : BulletBehaviour
{
    [SerializeField]
    private float _velocity = 1f;

    private Vector2 _dir;

    private void Update()
    {
        rb.velocity = transform.up * _velocity;
    }
}
