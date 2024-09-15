using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPickupsOnDeath : MonoBehaviour
{
    [System.Serializable]
    public struct PickupSpawn
    {
        [SerializeField]
        private Pickup _pickup;
        public Pickup Pickup => _pickup;

        [SerializeField]
        private int _maxCount;
        public int MaxCount => _maxCount;

        [SerializeField]
        private float _chance;
        public float Chance => _chance;
    }

    [SerializeField]
    private PickupSpawn[] _pickupSpawns;

    private DamageReceiver _dr;
    protected DamageReceiver Dr
    {
        get
        {
            if (_dr == null) _dr = GetComponentInParent<DamageReceiver>();
            return _dr;
        }
    }

    private void OnEnable()
    {
        Dr.OnDamage += SpawnPickups;
    }

    private void OnDisable()
    {
        Dr.OnDamage -= SpawnPickups;
    }

    private void SpawnPickups(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        if (!result.Killed) return;

        foreach (PickupSpawn spawn in _pickupSpawns)
        {
            for(int i = 0; i < spawn.MaxCount; i++)
            {
                if (Random.Range(0f, 1f) <= spawn.Chance)
                {
                    Instantiate(spawn.Pickup, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
