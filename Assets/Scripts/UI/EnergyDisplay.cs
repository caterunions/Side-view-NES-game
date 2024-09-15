using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    private PlayerStats _playerStats;

    private ShipWeaponHolder _shipWeaponHolder;

    [SerializeField]
    private Image _energyBar;

    [SerializeField]
    private Image _energyBarEmpty;

    [SerializeField]
    private TextMeshProUGUI _heavyAttackCostText;

    private float _energyBarMaxPercentage;

    private void OnEnable()
    {
        _playerStats = GameManager.Instance.Player.GetComponent<PlayerStats>();
        _shipWeaponHolder = GameManager.Instance.Player.GetComponentInChildren<ShipWeaponHolder>();

        _heavyAttackCostText.text = $"X{_shipWeaponHolder.HeavyEnergyCost}";

        _energyBarMaxPercentage = _playerStats.MaxEnergy / 16f;
        
        _energyBarEmpty.fillAmount = _energyBarMaxPercentage;
        _energyBar.fillAmount = _energyBarMaxPercentage;

        _playerStats.OnEnergyChange += UpdateEnergyBar;

        UpdateEnergyBar(_playerStats, 0);
    }

    private void OnDisable()
    {
        _playerStats.OnEnergyChange -= UpdateEnergyBar;
    }

    private void UpdateEnergyBar(PlayerStats stats, float change)
    {
        float energyPercentage = stats.CurrentEnergy / stats.MaxEnergy;

        _energyBar.fillAmount = energyPercentage * _energyBarMaxPercentage;
    }
}
