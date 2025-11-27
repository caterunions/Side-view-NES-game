using System.Collections;
using UnityEngine;

public class EnemyChasePlayer : EnemyAction
{
    [SerializeField]
    private EnemyMove _mover;

    [SerializeField]
    private float _forwardsSpeed;

    [SerializeField]
    private float _backwardsSpeed;

    [SerializeField]
    private float _maxDist;

    [SerializeField]
    private float _minDist;

    protected override IEnumerator ActionInstructions()
    {
        _mover.ChasePlayer(_minDist, _maxDist, _forwardsSpeed, _backwardsSpeed);
        yield return null;
    }
}
