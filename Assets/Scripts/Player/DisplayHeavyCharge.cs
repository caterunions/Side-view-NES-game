using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHeavyCharge : MonoBehaviour
{
    [SerializeField]
    private ShipWeaponHolder _shipWeaponHolder;

    [SerializeField]
    private SpriteRenderer _heavyChargeFX;

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
        _heavyChargeFX.transform.localScale = new Vector3(_shipWeaponHolder.ChargePercentage, _shipWeaponHolder.ChargePercentage, 1);
    }
}
