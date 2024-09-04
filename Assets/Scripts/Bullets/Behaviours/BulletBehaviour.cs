using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Bullet))]
public abstract class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;
    protected Rigidbody2D rb
    {
        get
        {
            if (_rb == null) _rb = GetComponent<Rigidbody2D>();
            return _rb;
        }
    }

    private Bullet _bullet;
    protected Bullet bullet
    {
        get
        {
            if (_bullet == null) _bullet = GetComponent<Bullet>();
            return _bullet;
        }
    }
}
