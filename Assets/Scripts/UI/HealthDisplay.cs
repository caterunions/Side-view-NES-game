using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private HealthPool _healthPool;

    [SerializeField]
    private Image _healthBar;

    [SerializeField]
    private Image _healthBarEmpty;

    private float _healthBarMaxPercentage;

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        _healthPool = GameManager.Instance.Player.GetComponent<HealthPool>();

        _healthBarMaxPercentage = _healthPool.MaxHealth / 16f;
        _healthBarEmpty.fillAmount = _healthBarMaxPercentage;
        _healthBar.fillAmount = _healthBarMaxPercentage;

        _healthPool.OnHPChange += UpdateHealthBar;

        UpdateHealthBar(_healthPool, 0);
    }

    private void Start()
    {
        OnEnable();
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
