using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticlesOnDeath : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    private DamageReceiver _dr;
    protected DamageReceiver Dr
    {
        get
        {
            if (_dr == null) _dr = GetComponentInParent<DamageReceiver>();
            return _dr;
        }
    }

    private void OnEnable()
    {
        Dr.OnDamage += SpawnParticles;
    }

    private void OnDisable()
    {
        Dr.OnDamage -= SpawnParticles;
    }

    private void SpawnParticles(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        if (!result.Killed) return;

        Instantiate(_particleSystem, transform.position, Quaternion.identity);
    }
}
