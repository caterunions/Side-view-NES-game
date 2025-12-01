using System.Collections;
using UnityEngine;

public class EnemyMoveToPoint : EnemyAction
{
    [SerializeField]
    private Vector2 _point;

    [SerializeField]
    private EnemyMove _mover;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private bool _stallUntilComplete = false;

    protected override IEnumerator ActionInstructions()
    {
        _mover.MoveToPoint(_point, _speed);
        if (_stallUntilComplete)
        {
            yield return new WaitUntil(() => _mover.MovementMode == EnemyMovementMode.None);
        }
        else
        {
            yield return null;
        }
    }
}
