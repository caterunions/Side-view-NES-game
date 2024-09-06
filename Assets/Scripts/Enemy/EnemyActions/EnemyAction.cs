using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : MonoBehaviour
{
    private EnemyBrain _enemyBrain;
    protected EnemyBrain EnemyBrain
    {
        get
        {
            if (_enemyBrain == null) _enemyBrain = GetComponentInParent<EnemyBrain>();
            return _enemyBrain;
        }
    }

    private EntityStats _stats;
    protected EntityStats Stats
    {
        get
        {
            if (_stats == null) _stats = GetComponentInParent<EntityStats>();
            return _stats;
        }
    }

    private bool _inProgress = false;
    public bool InProgress
    {
        get { return _inProgress; }
        private set { _inProgress = value; }
    }

    [SerializeField]
    protected float _startDelay;
    [SerializeField]
    protected float _endDelay;

    protected Coroutine _actionRoutine;

    public bool Act()
    {
        if (_actionRoutine == null)
        {
            InProgress = true;
            _actionRoutine = StartCoroutine(ActionRoutine());
            return true;
        }
        InProgress = false;
        return false;
    }

    public void Stop()
    {
        if (_actionRoutine == null) return;
        StopAllCoroutines();
        InProgress = false;
        _actionRoutine = null;
        ExtraStopInstructions();
    }

    protected virtual void ExtraStopInstructions()
    {

    }

    protected IEnumerator ActionRoutine()
    {
        yield return new WaitForSeconds(_startDelay);

        yield return StartCoroutine(ActionInstructions());

        yield return new WaitForSeconds(_endDelay);

        InProgress = false;
        _actionRoutine = null;
    }

    protected abstract IEnumerator ActionInstructions();
}
