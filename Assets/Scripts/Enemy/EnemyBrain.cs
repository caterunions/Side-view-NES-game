using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBrain : MonoBehaviour
{
    private Rigidbody2D _rb;
    public Rigidbody2D Rb
    {
        get
        {
            if(_rb == null) _rb = GetComponent<Rigidbody2D>();
            return _rb;
        }
    }

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

    [SerializeField]
    private GameObject _player;
    public GameObject Player => _player;

    [SerializeField]
    private EnemyAim _aimer;
    public EnemyAim Aimer => _aimer;
}