using System;
using UnityEngine;

public enum EnemyMovementMode
{
    None,
    ChasePlayer,
    FollowSpline,
    MoveToPoint
}

public class EnemyMove : MonoBehaviour
{
    public event Action<EnemyMove> OnSplineEndReached;

    [SerializeField]
    private Rigidbody2D _rb;

    public EnemyMovementMode MovementMode { get; private set; } = EnemyMovementMode.None;

    // player chasing stuff
    private Transform _player;
    public Transform Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private float _forwardChaseSpeed;
    private float _backwardChaseSpeed;
    private float _minDist;
    private float _maxDist;

    // spline stuff
    private SplineContainer _spline;

    private float _speed = 1.0f;
    private int _splineTargetIndex = 0;
    private bool _splineEndReached;
    public bool SplineEndReached => _splineEndReached;
    public bool FlipSpline { get; set; } = false;

    private Vector2 _curSplineTarget
    {
        get
        {
            if (FlipSpline) return new Vector2(_spline.CalculatedPoints[_splineTargetIndex].x * -1, _spline.CalculatedPoints[_splineTargetIndex].y);
            return _spline.CalculatedPoints[_splineTargetIndex];
        }
    }

    private Vector2 _moveToTarget;

    public void FollowSpline(SplineContainer spline, float speed)
    {
        _speed = speed;
        MovementMode = EnemyMovementMode.FollowSpline;
        _splineEndReached = false;
        _spline = spline;
        _splineTargetIndex = 0;
        transform.position = _curSplineTarget;
    }

    public void ChasePlayer(float minDist, float maxDist, float forwardSpeed, float backwardSpeed)
    {
        MovementMode = EnemyMovementMode.ChasePlayer;
        _forwardChaseSpeed = forwardSpeed;
        _backwardChaseSpeed = backwardSpeed;
        _minDist = minDist;
        _maxDist = maxDist;
    }

    public void MoveToPoint(Vector2 point, float speed)
    {
        MovementMode = EnemyMovementMode.MoveToPoint;
        _speed = speed;
        _moveToTarget = point;
    }

    public void CancelMovement()
    {
        MovementMode = EnemyMovementMode.None;
    }

    private void FixedUpdate()
    {
        switch (MovementMode)
        {
            case EnemyMovementMode.FollowSpline:
                SplineMovement();
                break;
            case EnemyMovementMode.ChasePlayer:
                ChasePlayerMovement();
                break;
            case EnemyMovementMode.MoveToPoint:
                MoveToPointMovement();
                break;
            default:
                break;
        }
    }

    private void SplineMovement()
    {
        if (_spline == null || _splineEndReached) return;

        if (Vector2.Distance(transform.position, _curSplineTarget) < 0.1f)
        {
            if (_splineTargetIndex < _spline.CalculatedPoints.Count - 1)
            {
                _splineTargetIndex++;
            }
            else
            {
                MovementMode = EnemyMovementMode.None;
                _rb.linearVelocity = Vector2.zero;
                _splineEndReached = true;
                OnSplineEndReached?.Invoke(this);
                return;
            }
        }

        Vector2 toTarg = (_curSplineTarget - (Vector2)transform.position).normalized;
        _rb.linearVelocity = toTarg * _speed;
    }

    private void ChasePlayerMovement()
    {
        Vector2 toTarg = ((Vector2)_player.position - (Vector2)transform.position).normalized;
        float dist = Vector2.Distance(transform.position, _player.position);

        if (dist > _maxDist)
        {
            _rb.linearVelocity = toTarg * _forwardChaseSpeed;
        }
        else if (dist < _minDist && Mathf.Abs(transform.position.x) < 17 && Mathf.Abs(transform.position.y) < 17)
        {
            _rb.linearVelocity = toTarg * -_backwardChaseSpeed;
        }
        else
        {
            _rb.linearVelocity *= 0.9f;
        }
    }

    private void MoveToPointMovement()
    {
        if (Vector2.Distance(transform.position, _moveToTarget) < 0.1f)
        {
            MovementMode = EnemyMovementMode.None;
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 toTarg = (_moveToTarget - (Vector2)transform.position).normalized;
        _rb.linearVelocity = toTarg * _speed;
    }
}
