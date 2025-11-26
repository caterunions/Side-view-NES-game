using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : BulletBehaviour
{
    [SerializeField]
    [Tooltip("Rotation speed in degrees per second")]
    private float _rotationSpeed = 10f;

    private void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
}
