using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashHurtOverlayOnPlayerDamage : MonoBehaviour
{
    private DamageReceiver _dr;

    [SerializeField]
    private Image _hurtOverlay;

    [SerializeField]
    private float _fadeTime = 1f;

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        _dr = GameManager.Instance.Player.GetComponentInChildren<HealthDamageReceiver>();

        _dr.OnDamage += FlashHurtOverlay;
    }

    private void Start()
    {
        OnEnable();
    }

    private void OnDisable()
    {
        _dr.OnDamage -= FlashHurtOverlay;
    }

    private void FlashHurtOverlay(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        Color overlayColor = _hurtOverlay.color;
        overlayColor.a += 0.5f;
        _hurtOverlay.color = overlayColor;
    }

    private void Update()
    {
        Color overlayColor = _hurtOverlay.color;
        if(overlayColor.a > 0f) overlayColor.a -= Time.deltaTime / _fadeTime;
        _hurtOverlay.color = overlayColor;
    }
}
