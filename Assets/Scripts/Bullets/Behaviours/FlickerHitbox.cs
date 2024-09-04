using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerHitbox : BulletBehaviour
{
    [SerializeField]
    private Collider2D _hitbox;

    [SerializeField]
    private float _flickerRate;

    private Coroutine _flickerRoutine;

    private void OnEnable()
    {
        _flickerRoutine = StartCoroutine(FlickerRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(_flickerRoutine);
        _flickerRoutine = null;
    }

    private IEnumerator FlickerRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_flickerRate);
            _hitbox.enabled = false;
            yield return null;
            _hitbox.enabled = true;
        }
    }
}
