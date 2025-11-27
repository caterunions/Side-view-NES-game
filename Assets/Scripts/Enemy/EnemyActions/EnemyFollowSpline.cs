using System.Collections;
using UnityEngine;

public class EnemyFollowSpline : EnemyAction
{
    [SerializeField]
    private SplineContainer _spline;

    [SerializeField]
    private EnemyMove _mover;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private bool _stallUntilComplete = false;

    protected override IEnumerator ActionInstructions()
    {
        _mover.FollowSpline(_spline, _speed);
        if (_stallUntilComplete)
        {
            yield return new WaitUntil(() => _mover.SplineEndReached);
        }
        else
        {
            yield return null;
        }
    }
}
