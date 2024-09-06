using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    public event Action<PlayerStats, float> OnEnergyChange;

    [SerializeField]
    private int _maxEnergy;
    public int MaxEnergy => _maxEnergy;

    private float _currentEnergy;
    public float CurrentEnergy
    {
        get 
        {
            return Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
        } 
        set
        {
            _currentEnergy = value;
            Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
            OnEnergyChange?.Invoke(this, value);
        }
    }

    [SerializeField]
    private float _energyRegen;

    private void OnEnable()
    {
        _currentEnergy = 0;
    }

    private void Update()
    {
        if(_currentEnergy < _maxEnergy)
        {
            CurrentEnergy += _energyRegen * Time.deltaTime;
        }
    }
}
