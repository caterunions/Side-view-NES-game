using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
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
        Dr.OnDamage += TryDestroy;
    }

    private void OnDisable()
    {
        Dr.OnDamage -= TryDestroy;
    }

    private void TryDestroy(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        if (!result.Killed) return;

        Destroy(gameObject);
    }
}
