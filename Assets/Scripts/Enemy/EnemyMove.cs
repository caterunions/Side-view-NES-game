using System;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public event Action<EnemyMove> OnEndReached;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private float _speed = 1.0f;

    private SplineContainer _spline;

    private int _targetIndex = 0;
    private bool _endReached;
    public bool FlipSpline { get; set; } = false;

    private Vector2 _curTarget
    {
        get 
        {
            if (FlipSpline) return new Vector2(_spline.CalculatedPoints[_targetIndex].x * -1, _spline.CalculatedPoints[_targetIndex].y);
            return _spline.CalculatedPoints[_targetIndex]; 
        }
    }

    public void FollowSpline(SplineContainer spline)
    {
        _endReached = false;
        _spline = spline;
        _targetIndex = 0;
        transform.position = _curTarget;
    }

    private void Update()
    {
        if(_spline == null || _endReached) return;

        if(Vector2.Distance(transform.position, _curTarget) < 0.1f)
        {
            if(_targetIndex < _spline.CalculatedPoints.Count - 1)
            {
                _targetIndex++;
            }
            else
            {
                _endReached = true;
                OnEndReached?.Invoke(this);
                _rb.linearVelocity = Vector2.zero;
            }
        }

        Vector2 toTarg = (_curTarget - (Vector2)transform.position).normalized;
        _rb.linearVelocity = toTarg * _speed;
    }
}
