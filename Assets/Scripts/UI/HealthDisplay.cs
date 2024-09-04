using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField]
    private HealthPool _healthPool;

    [SerializeField]
    private Image _healthBar;

    [SerializeField]
    private Image _healthBarEmpty;

    private float _healthBarMaxPercentage;

    private void OnEnable()
    {
        _healthBarMaxPercentage = _healthPool.MaxHealth / 16;
        _healthBarEmpty.fillAmount = _healthBarMaxPercentage;
        _healthBar.fillAmount = _healthBarMaxPercentage;

        _healthPool.OnHPChange += UpdateHealthBar;

        UpdateHealthBar(_healthPool, 0);
    }

    private void OnDisable()
    {
        _healthPool.OnHPChange -= UpdateHealthBar;
    }

    private void UpdateHealthBar(HealthPool pool, float damage)
    {
        float hpPercentage = pool.Health / pool.MaxHealth;

        _healthBar.fillAmount = hpPercentage * _healthBarMaxPercentage;
    }
}
