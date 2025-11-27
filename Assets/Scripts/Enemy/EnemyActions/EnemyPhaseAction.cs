using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhaseLoopBehaviour
{
    Ordered,
    OrderedOnceThenRandom,
    Random
}

public class EnemyPhaseAction : EnemyAction
{
    [Header("Phase Change Behaviour")]
    [SerializeField]
    private HealthPool _healthPool;

    [SerializeField, Range(0, 1)]
    [Tooltip("Percentage of health to end phase at")]
    private float _phaseChangeThreshold = 1.0f;

    [SerializeField]
    private bool _interruptActionOnThresholdMet = false;

    [Header("Phase Loop Behaviour")]
    [SerializeField]
    private PhaseLoopBehaviour _loopBehaviour;

    [Header("Phase Actions")]
    [SerializeField]
    private EnemyAction _phaseStartAction;

    [SerializeField]
    private EnemyAction _phaseEndAction;

    [SerializeField]
    private List<EnemyAction> _phaseActions;

    private EnemyAction _curAction;

    private int _curActionIndex = 0;

    private bool _looped = false;

    private bool _actionInterrupted = false;

    protected override IEnumerator ActionInstructions()
    {
        if (_phaseStartAction != null)
        {
            _phaseStartAction.Act();
            yield return new WaitUntil(() => !_phaseStartAction.InProgress);
        }

        while (_healthPool.Health / _healthPool.MaxHealth > _phaseChangeThreshold)
        {
            switch (_loopBehaviour)
            {
                case PhaseLoopBehaviour.Ordered:
                    _curAction = _phaseActions[_curActionIndex];

                    _curActionIndex++;

                    if (_curActionIndex >= _phaseActions.Count) _curActionIndex = 0;
                    break;

                case PhaseLoopBehaviour.OrderedOnceThenRandom:
                    // ordered selection
                    if (!_looped)
                    {
                        _curAction = _phaseActions[_curActionIndex];

                        _curActionIndex++;

                        // we have run through all the actions
                        if (_curActionIndex >= _phaseActions.Count) _looped = true;
                    }
                    // random after first loop through
                    else
                    {
                        _curAction = _phaseActions[RandomActionIndex()];
                    }
                    break;

                case PhaseLoopBehaviour.Random:
                    EnemyAction action = _phaseActions[RandomActionIndex()];
                    break;
            }
            // perform action
            _curAction.Act();
            // wait until completed or interrupted
            yield return new WaitUntil(() => !_curAction.InProgress);
        }

        if (_phaseEndAction != null)
        {
            _phaseEndAction.Act();
            yield return new WaitUntil(() => !_phaseEndAction.InProgress);
        }
    }

    private void Update()
    {
        if (!_interruptActionOnThresholdMet || _actionInterrupted) return;

        if (_healthPool.Health / _healthPool.MaxHealth > _phaseChangeThreshold)
        {
            _actionInterrupted = true;
            _curAction.Stop();
        }
    }

    private int RandomActionIndex()
    {
        // choose first action if only one in list
        if (_phaseActions.Count == 1) return 0;

        bool found = false;
        int newIndex = 0;
        while (!found)
        {
            newIndex = UnityEngine.Random.Range(0, _phaseActions.Count - 1);
            found = newIndex != _curActionIndex;
        }
        // store for more random selection later
        _curActionIndex = newIndex;
        return newIndex;
    }
}
