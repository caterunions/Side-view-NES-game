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
    private SplineContainer _splineContainer;

    [SerializeField]
    private bool _moveOnStart = true;

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

    private float _delayMoveTime;
    private float _accumulatedMoveTime;
    private bool _moved = false;

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

        _bulletMLPatternManager = bulletMLPatternManager;

        _spawner = spawner;

        _mover.FlipSpline = flipSpline;

        _delayMoveTime = delay;
    }

    protected virtual void OnEnable()
    {
        _mover.OnEndReached += HandleSplineEndReached;

        _actionIndex = 0;
        if (_startAction != null)
        {
            _curAction = _startAction;
            _curAction.Act();
        }
        else _curAction = _actions[_actionIndex];
    }

    protected virtual void OnDisable()
    {
        _mover.OnEndReached -= HandleSplineEndReached;

        StopAllCoroutines();

        _curAction.Stop();
    }

    protected virtual void Update()
    {
        if (!_moved && _accumulatedMoveTime >= _delayMoveTime && _moveOnStart)
        {
            _mover.FollowSpline(_splineContainer);
            _moved = true;
        }
        else
        {
            _accumulatedMoveTime += Time.deltaTime;
        }

        if (_curAction.InProgress == false)
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