using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponHolder : MonoBehaviour
{
    public event Action<ShipWeaponHolder> OnChargePercentageChange;

    [SerializeField]
    private Attack _lightAttack;
    public Attack LightAttack => _lightAttack;

    [SerializeField]
    private Attack _heavyAttack;
    public Attack HeavyAttack => _heavyAttack;

    [SerializeField]
    private float _heavyChargeTime;
    public float HeavyChargeTime => _heavyChargeTime;

    
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
}
