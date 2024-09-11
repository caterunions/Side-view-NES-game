using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashWhiteOnDamage : MonoBehaviour
{
    [SerializeField]
    private GameObject _maskedWhiteSprite;

    [SerializeField]
    private float _flashDuration = 0.05f;

    private DamageReceiver _dr;
    protected DamageReceiver Dr
    {
        get
        {
            if (_dr == null) _dr = GetComponentInParent<DamageReceiver>();
            return _dr;
        }
    }

    private float _currentFlashTimeRemaining = 0f;

    private void OnEnable()
    {
        _maskedWhiteSprite.SetActive(false);

        Dr.OnDamage += FlashWhiteSprite;
    }

    private void OnDisable()
    {
        Dr.OnDamage -= FlashWhiteSprite;
    }

    private void FlashWhiteSprite(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        _currentFlashTimeRemaining = _flashDuration;
    }

    private void Update()
    {
        _maskedWhiteSprite.SetActive(_currentFlashTimeRemaining > 0);

        if(_currentFlashTimeRemaining > 0) _currentFlashTimeRemaining -= Time.deltaTime;
    }
}