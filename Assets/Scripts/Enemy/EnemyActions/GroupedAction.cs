using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroupedAction : EnemyAction
{
    [SerializeField]
    private List<EnemyAction> _groupedActions;
    public List<EnemyAction> GroupedActions => _groupedActions;

    protected override IEnumerator ActionInstructions()
    {
        foreach(EnemyAction a in _groupedActions)
        {
            a.Act();
        }

        yield return new WaitUntil(() => _groupedActions.All(a => !a.InProgress));
    }

    protected override void ExtraStopInstructions()
    {
        foreach (EnemyAction a in _groupedActions)
        {
            a.Stop();
        }
    }
}