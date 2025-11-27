using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public event Action<EnemyBrain> OnGameObjectDestroy;

    private HealthPool _healthPool;
    protected HealthPool healthPool
    {
        get
        {
            if (_healthPool == null) _healthPool = GetComponent<HealthPool>();
            return _healthPool;
        }
    }

    private HealthDamageReceiver _damageReceiver;
    public HealthDamageReceiver DamageReceiver
    {
        get
        {
            if (_damageReceiver == null) _damageReceiver = GetComponent<HealthDamageReceiver>();
            return _damageReceiver;
        }
    }

    [SerializeField]
    private bool _destroyOnSplineEnd = true;

    [SerializeField]
    private EnemyMove _mover;
    public EnemyMove Mover => _mover;

    [SerializeField]
    private BulletLauncher _launcher;

    [SerializeField]
    private EnemyAction _startAction;

    [SerializeField]
    private List<EnemyAction> _actions;

    protected GameObject _player;

    [SerializeField]
    private EnemyAim _aimer;
    public EnemyAim Aimer => _aimer;

    protected EnemySpawner _spawner;

    private BulletMLPatternManager _bulletMLPatternManager;
    public BulletMLPatternManager BulletMLPatternManager => _bulletMLPatternManager;

    [SerializeField]
    private int _scoreReward;
    public int ScoreReward => _scoreReward;

    private EnemyAction _curAction;
    private int _actionIndex = 0;

    private float _delayInitTime;
    private float _accumulatedInitTime;

    private bool _startActionQueued = false;

    public void LockAimer()
    {
        Aimer.Locked = true;
    }

    public void UnlockAimer()
    {
        Aimer.Locked = false;
    }

    public void Initialize(GameObject player, EnemySpawner spawner, BulletMLPatternManager bulletMLPatternManager, bool flipSpline, float delay)
    {
        _player = player;

        Aimer.Player = _player.transform;
        _mover.Player = _player.transform;

        _bulletMLPatternManager = bulletMLPatternManager;

        _spawner = spawner;

        _mover.FlipSpline = flipSpline;

        _delayInitTime = delay;
    }

    private void OnDestroy()
    {
        OnGameObjectDestroy?.Invoke(this);
    }

    protected virtual void OnEnable()
    {
        _mover.OnSplineEndReached += HandleSplineEndReached;

        _actionIndex = 0;
        if (_startAction != null)
        {
            _curAction = _startAction;
            _startActionQueued = true;
        }
        else _curAction = _actions[_actionIndex];
    }

    protected virtual void OnDisable()
    {
        _mover.OnSplineEndReached -= HandleSplineEndReached;

        StopAllCoroutines();

        _curAction.Stop();
    }

    protected virtual void Update()
    {
        _accumulatedInitTime += Time.deltaTime;

        if (_accumulatedInitTime < _delayInitTime) return;

        if (_startActionQueued)
        {
            _curAction.Act();
            _startActionQueued = false;
        }

        if (!_curAction.InProgress)
        {
            if (_curAction != _actions[_actionIndex] && !_actions[_actionIndex].InProgress)
            {
                _curAction = _actions[_actionIndex];
            }

            _curAction.Act();

            if (_actionIndex < _actions.Count - 1) _actionIndex++;
            else _actionIndex = 0;
        }

        _launcher.Blocked = (
            transform.position.x > 17 ||
            transform.position.x < -17 ||
            transform.position.y > 17 ||
            transform.position.y < -17);
    }

    protected void HandleSplineEndReached(EnemyMove mover)
    {
        if (_destroyOnSplineEnd)
        {
            _spawner.DestroyEnemy(this, false);
        }
    }
}