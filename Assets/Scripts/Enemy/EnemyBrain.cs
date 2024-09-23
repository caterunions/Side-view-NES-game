using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    private HealthPool _healthPool;
    protected HealthPool healthPool
    {
        get
        {
            if (_healthPool == null) _healthPool = GetComponent<HealthPool>();
            return _healthPool;
        }
    }

    [SerializeField]
    private EnemyAction _startAction;

    [SerializeField]
    private List<EnemyAction> _actions;

    private GameObject _player;

    [SerializeField]
    private EnemyAim _aimer;
    public EnemyAim Aimer => _aimer;

    public EnemySpawner Spawner { get; set; }

    [SerializeField]
    private int _scoreReward;
    public int ScoreReward => _scoreReward;

    private EnemyAction _curAction;
    private int _actionIndex = 0;

    public void LockAimer()
    {
        Aimer.Locked = true;
    }

    public void UnlockAimer()
    {
        Aimer.Locked = false;
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;

        Aimer.Player = _player.transform;
    }

    private void OnEnable()
    {
        _actionIndex = 0;
        if (_startAction != null)
        {
            _curAction = _startAction;
            _curAction.Act();
        }
        else _curAction = _actions[_actionIndex];
    }

    private void OnDisable()
    {
        _curAction.Stop();
    }

    private void Update()
    {
        if(_curAction.InProgress == false)
        {
            if(_curAction != _actions[_actionIndex] && !_actions[_actionIndex].InProgress)
            {
                _curAction = _actions[_actionIndex];
            }

            _curAction.Act();

            if (_actionIndex < _actions.Count - 1) _actionIndex++;
            else _actionIndex = 0;
        }
    }
}