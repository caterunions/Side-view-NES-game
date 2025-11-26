using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedAcceleratingForwardVelocity : AcceleratingForwardVelocity
{
    [SerializeField]
    private float _startingVariance;

    [SerializeField]
    private float _endingVariance;

    protected override void OnEnable()
    {
        _startVelocity += Random.Range(-_startingVariance, _startingVariance);
        _endVelocity += Random.Range(-_endingVariance, _endingVariance);

        base.OnEnable();
    }
}
