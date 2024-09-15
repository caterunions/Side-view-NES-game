using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    protected override void PickupEffect(PlayerStats stats)
    {
        HealthPool pool = stats.GetComponent<HealthPool>();
        if (pool != null)
        {
            pool.Heal(1f);
        }
    }
}
