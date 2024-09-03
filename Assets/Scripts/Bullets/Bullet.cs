using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public event Action<Bullet, DamageReceiver, DamageEvent> OnHit;

    [SerializeField]
    private List<BulletBehaviour> _behaviours = new List<BulletBehaviour>();
    public List<BulletBehaviour> Behaviours => _behaviours;

    [SerializeField]
    private int _solidObjectPierceCount = 0;

    private GameObject _spawner;
    public GameObject Spawner => _spawner;

    [SerializeField]
    private float _range = 10;
    public float Range => _range;

    [SerializeField]
    private Collider2D _collider;

    [SerializeField]
    private bool _destroyOnWalls = true;

    [SerializeField]
    private float _damage = 0;
    public float Damage => _damage;

    public float DistanceTravelled
    {
        get
        {
            if (_origin == null) return 0;
            return Vector2.Distance(_origin, transform.position);
        }
    }

    private DamageTeam _team;
    public DamageTeam Team => _team;

    private BulletLauncher _launcher;
    public BulletLauncher Launcher => _launcher;

    private Vector2 _origin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall") && _destroyOnWalls)
        {
            Destroy(gameObject);
        }

        DamageReceiver dr = collision.GetComponentInParent<DamageReceiver>();
        if(dr != null)
        {
            if(dr.Team != Team)
            {
                float damage = Damage;

                DamageEvent dmgEvent = new DamageEvent(damage, _spawner, gameObject);
                dr.ReceiveDamage(dmgEvent);
                OnHit?.Invoke(this, dr, dmgEvent);

                if (!collision.isTrigger)
                {
                    if (_solidObjectPierceCount <= 0) Destroy(gameObject);
                    _solidObjectPierceCount--;
                }
            }
        }
        else if (!collision.isTrigger)
        {
            if(_solidObjectPierceCount <= 0) Destroy(gameObject);
            _solidObjectPierceCount--;
        }
    }

    private void OnDestroy()
    {
        OnHit = null;
    }

    protected virtual void Awake()
    {
        if(_collider != null) _collider.enabled = false;
        Behaviours.ForEach(b => b.enabled = false);
        _origin = transform.position;
    }

    public virtual void Initialize(GameObject spawner, BulletLauncher launcher, DamageTeam team)
    {
        _spawner = spawner;
        _launcher = launcher;
        _team = team;

        if (_collider != null) _collider.enabled = true;
        Behaviours.ForEach(_b => _b.enabled = true);
    }

    private void Update()
    {
        if (DistanceTravelled >= _range) Destroy(gameObject);
    }
}
