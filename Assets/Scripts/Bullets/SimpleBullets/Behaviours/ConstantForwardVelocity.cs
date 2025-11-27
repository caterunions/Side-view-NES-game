using UnityEngine;

public class ConstantForwardVelocity : BulletBehaviour
{
    [SerializeField]
    private float _velocity = 1f;

    private void Update()
    {
        rb.linearVelocity = transform.up * _velocity;
    }
}
