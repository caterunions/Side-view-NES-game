using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPool : MonoBehaviour
{
    public event Action<HealthPool, float> OnHPChange;

    [SerializeField]
    private EntityStats _stats;

    private float _health;
    public float Health => _health;

    public float MaxHealth
    {
        get { return _stats.MaxHealth; }
    }

    private void OnEnable()
    {
        _health = MaxHealth;
        OnHPChange?.Invoke(this, 0);
    }

    public float Damage(float damage)
    {
        if(_health < damage)
        {
            damage -= _health;
            _health = 0;
        }
        else
        {
            _health -= damage;
        }

        OnHPChange?.Invoke(this, damage);

        return damage;
    }

    public float Heal(float heal)
    {
        if(_health + heal > MaxHealth)
        {
            heal -= (MaxHealth - _health);
            _health = MaxHealth;
        }
        else
        {
            _health += heal;
        }

        OnHPChange?.Invoke(this, heal);

        return heal;
    }
}
