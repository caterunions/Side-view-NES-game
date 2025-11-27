using System.Collections;
using UnityEngine;

public class EnemyStopMovement : EnemyAction
{
    [SerializeField]
    private EnemyMove _mover;

    protected override IEnumerator ActionInstructions()
    {
        _mover.CancelMovement();
        yield return null;
    }
}
