public class EnergyPickup : Pickup
{
    protected override void PickupEffect(PlayerStats stats)
    {
        stats.AddEnergy(1f);
    }
}
