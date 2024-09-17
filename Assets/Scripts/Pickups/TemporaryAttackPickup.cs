using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryAttackPickup : Pickup
{
    [SerializeField]
    private Attack _tempAttack;

    [SerializeField]
    private Sprite _powerUpSprite;

    [SerializeField]
    private float _duration;

    protected override void PickupEffect(PlayerStats stats)
    {
        ShipWeaponHolder weaponHolder = stats.GetComponentInChildren<ShipWeaponHolder>();
        if (weaponHolder != null)
        {
            weaponHolder.ApplyNewPowerUpLightAttack(_tempAttack, _powerUpSprite, _duration);
        }
    }
}
