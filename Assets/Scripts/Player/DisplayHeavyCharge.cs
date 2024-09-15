using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHeavyCharge : MonoBehaviour
{
    private PlayerStats _stats;
    protected PlayerStats stats
    {
        get
        {
            if (_stats == null) _stats = GetComponentInParent<PlayerStats>();
            return _stats;
        }
    }

    [SerializeField]
    private ShipWeaponHolder _shipWeaponHolder;

    [SerializeField]
    private SpriteRenderer _heavyChargeFX;

    [SerializeField]
    private SpriteRenderer _chargeAttackReadyOutline;

    [SerializeField]
    private float _maxOutlineAlpha = 1f;

    [SerializeField]
    private float _fadeTime = 1f;

    [SerializeField]
    private ParticleSystem _chargeCompleteParticles;

    private bool _chargeComplete = false;
    private float _curOutlineAlpha = 0f;
    private bool _alphaIncreasing = true;

    private void OnEnable()
    {
        HandleChargeChange(_shipWeaponHolder);

        _shipWeaponHolder.OnChargePercentageChange += HandleChargeChange;
    }

    private void OnDisable()
    {
        _shipWeaponHolder.OnChargePercentageChange -= HandleChargeChange;
    }

    private void HandleChargeChange(ShipWeaponHolder shipWeaponHolder)
    {
        if(_shipWeaponHolder.ChargePercentage == 1 && !_chargeComplete)
        {
            Instantiate(_chargeCompleteParticles, _heavyChargeFX.transform.position, Quaternion.identity);
            _chargeComplete = true;
        }
        else if(_shipWeaponHolder.ChargePercentage < 1f)
        {
            _chargeComplete = false;
        }

        _heavyChargeFX.transform.localScale = new Vector3(_shipWeaponHolder.ChargePercentage, _shipWeaponHolder.ChargePercentage, 1);
    }

    private void Update()
    {
        Color outlineColor = _chargeAttackReadyOutline.color;
        if (stats.CurrentEnergy >= _shipWeaponHolder.HeavyEnergyCost)
        {
            if(_alphaIncreasing)
            {
                _curOutlineAlpha += Time.deltaTime / _fadeTime;
                outlineColor.a = _curOutlineAlpha;
                if (_curOutlineAlpha >= _maxOutlineAlpha) _alphaIncreasing = false;
            }
            else
            {
                if(_curOutlineAlpha > 0.1f) _curOutlineAlpha -= Time.deltaTime / _fadeTime;
                else _curOutlineAlpha -= Time.deltaTime / _fadeTime / 20f;
                outlineColor.a = _curOutlineAlpha;
                if (_curOutlineAlpha <= 0) _alphaIncreasing = true;
            }
        }
        else
        {
            outlineColor.a = 0f;
            _curOutlineAlpha = 0f;
            _alphaIncreasing = true;
        }
        _chargeAttackReadyOutline.color = outlineColor;
    }
}
