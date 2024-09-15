using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : Pickup
{
    protected override void PickupEffect(PlayerStats stats)
    {
        stats.AddEnergy(1f);
    }
}
