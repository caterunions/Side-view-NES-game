using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    public event Action<BulletLauncher, Bullet, DamageReceiver, DamageEvent> OnSpawnedBulletHit;
    public event Action<BulletLauncher, Bullet> OnLaunch;

    private EntityStats _stats;
    private EntityStats stats
    {
        get
        {
            if (_stats == null) _stats = GetComponentInParent<EntityStats>();
            return _stats;
        }
    }

    public void Launch(PatternData pattern, float damageMultiplier)
    {
        float angleStep = pattern.Spread / pattern.Count;
        float aimAngle = pattern.FixedAngle == null ? transform.rotation.eulerAngles.z + pattern.AngleOffset : (float)pattern.FixedAngle + pattern.AngleOffset;
        float centeringOffset = (pattern.Spread / 2) - (angleStep / 2);

        for(int i = 0; i < pattern.Count; i++)
        {
            float currentBulletAngle = angleStep * i;

            aimAngle += UnityEngine.Random.Range(pattern.RandomAngleOffset * -1, pattern.RandomAngleOffset);

            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, aimAngle + currentBulletAngle - centeringOffset));

            Vector2 position = transform.position;
            if (pattern.Position != null) position = (Vector2)pattern.Position;

            Bullet b = Instantiate(pattern.Bullet, position, rotation);

            b.OnHit += TriggerSpawnedHitEvent;
            b.Initialize(transform.root.gameObject, this, pattern.Team);
        }
    }

    private void TriggerSpawnedHitEvent(Bullet bullet, DamageReceiver dr, DamageEvent dmgEvent)
    {
        OnSpawnedBulletHit?.Invoke(this, bullet, dr, dmgEvent);
    }
}

public class PatternData
{
    public PatternData(Bullet bullet, int count, float spread, float angleOffset, float randomAngleOffset, DamageTeam team, Vector2? position, float? fixedAngle)
    {
        _bullet = bullet;
        _count = count;
        _spread = spread;
        _angleOffset = angleOffset;
        _randomAngleOffset = randomAngleOffset;
        _team = team;
        _position = position;
        _fixedAngle = fixedAngle;
    }

    private Bullet _bullet;
    public Bullet Bullet => _bullet;

    private int _count;
    public int Count => _count;

    private float _spread;
    public float Spread => _spread;

    private float _angleOffset;
    public float AngleOffset => _angleOffset;

    private float _randomAngleOffset;
    public float RandomAngleOffset => _randomAngleOffset;

    private DamageTeam _team;
    public DamageTeam Team => _team;

    private Vector2? _position;
    public Vector2? Position => _position;

    private float? _fixedAngle;
    public float? FixedAngle => _fixedAngle;
}