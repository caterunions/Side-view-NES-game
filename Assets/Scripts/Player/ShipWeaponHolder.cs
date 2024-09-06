using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponHolder : MonoBehaviour
{
    public event Action<ShipWeaponHolder> OnChargePercentageChange;

    [SerializeField]
    private PlayerStats _playerStats;

    [Header("Light Attack")]
    [SerializeField]
    private Attack _lightAttack;
    public Attack LightAttack => _lightAttack;

    [Header("Heavy Attack")]
    [SerializeField]
    private Attack _heavyAttack;
    public Attack HeavyAttack => _heavyAttack;

    [SerializeField]
    private float _heavyChargeTime;
    public float HeavyChargeTime => _heavyChargeTime;

    [SerializeField]
    private int _heavyEnergyCost;
    public int HeavyEnergyCost => _heavyEnergyCost;
    
    private float _chargePercentage = 0;
    public float ChargePercentage
    {
        get { return _chargePercentage; }
        set
        {
            _chargePercentage = value;
            Mathf.Clamp01(_chargePercentage);
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
}
