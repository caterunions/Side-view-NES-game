using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingForwardVelocity : BulletBehaviour
{
    [SerializeField]
    protected float _startVelocity;

    [SerializeField] 
    protected float _endVelocity;

    [SerializeField]
    protected float _rampTime;

    protected float _endTime;

    protected virtual void OnEnable()
    {
        _endTime = Time.time + _rampTime;
    }

    private void Update()
    {
        rb.velocity = transform.up * Mathf.SmoothStep(_endVelocity, _startVelocity, (_endTime - Time.time) / _rampTime);
    }
}
