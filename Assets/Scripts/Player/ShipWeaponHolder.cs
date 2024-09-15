using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponHolder : MonoBehaviour
{
    public event Action<ShipWeaponHolder> OnChargePercentageChange;
    public event Action<ShipWeaponHolder> OnGainLightAttackPowerUp;
    public event Action<ShipWeaponHolder> OnLightAttackPowerUpEnd;

    [SerializeField]
    private PlayerStats _playerStats;

    [Header("Light Attack")]
    [SerializeField]
    private Attack _lightAttack;
    public Attack LightAttack
    {
        get
        {
            if (_powerUpLightAttack == null) return _lightAttack;
            return _powerUpLightAttack;
        }
    }

    [SerializeField]
    private Sprite _lightAttackSprite;
    public Sprite LightAttackSprite
    {
        get
        {
            if (_powerUpLightAttackSprite == null) return _lightAttackSprite;
            return _powerUpLightAttackSprite;
        }
    }

    [Header("Heavy Attack")]
    [SerializeField]
    private Attack _heavyAttack;
    public Attack HeavyAttack => _heavyAttack;

    [SerializeField]
    private Sprite _heavyAttackSprite;
    public Sprite HeavyAttackSprite => _heavyAttackSprite;

    [SerializeField]
    private float _heavyChargeTime;
    public float HeavyChargeTime => _heavyChargeTime;

    [SerializeField]
    private int _heavyEnergyCost;
    public int HeavyEnergyCost => _heavyEnergyCost;

    private Attack _powerUpLightAttack = null;

    private Sprite _powerUpLightAttackSprite = null;

    private float _powerUpEndTime = 0f;

    private float _powerUpTimeRemaining
    {
        get { return _powerUpEndTime - Time.time; }
    }
    
    private float _chargePercentage = 0;
    public float ChargePercentage
    {
        get { return _chargePercentage; }
        set
        {
            _chargePercentage = value;
            _chargePercentage = Mathf.Clamp01(_chargePercentage);
            OnChargePercentageChange?.Invoke(this);
        }
    }

    public bool CanFireHeavyWeapon
    {
        get { return _playerStats.CurrentEnergy >= _heavyEnergyCost; }
    }

    public void DeductHeavyWeaponCost()
    {
        _playerStats.CurrentEnergy -= _heavyEnergyCost;
    }

    public void ApplyNewPowerUpLightAttack(Attack powerUpAttack, Sprite powerUpSprite, float powerUpTime)
    {
        _powerUpLightAttack = powerUpAttack;
        _powerUpLightAttackSprite = powerUpSprite;
        _powerUpEndTime = Time.time + powerUpTime;
        OnGainLightAttackPowerUp?.Invoke(this);
    }

    private void Update()
    {
        if(_powerUpTimeRemaining <= 0f)
        {
            _powerUpLightAttack = null;
            _powerUpLightAttackSprite = null;
            OnLightAttackPowerUpEnd?.Invoke(this);
        }
    }
}
