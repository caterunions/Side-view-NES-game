using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAttackIcons : MonoBehaviour
{
    [SerializeField]
    private Image _lightAttackIcon;

    [SerializeField]
    private Image _heavyAttackIcon;

    [SerializeField]
    private Image _lightAttackPowerUpBorder;

    private ShipWeaponHolder _shipWeaponHolder;

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        _shipWeaponHolder = GameManager.Instance.Player.GetComponentInChildren<ShipWeaponHolder>();

        _shipWeaponHolder.OnGainLightAttackPowerUp += ChangeAttackIcons;
        _shipWeaponHolder.OnLightAttackPowerUpEnd += ChangeAttackIcons;
    }

    private void Start()
    {
        OnEnable();

        ChangeAttackIcons(_shipWeaponHolder);
    }

    private void OnDisable()
    {
        _shipWeaponHolder.OnGainLightAttackPowerUp -= ChangeAttackIcons;
        _shipWeaponHolder.OnLightAttackPowerUpEnd -= ChangeAttackIcons;
    }

    private void ChangeAttackIcons(ShipWeaponHolder weaponHolder)
    {
        _lightAttackIcon.sprite = _shipWeaponHolder.LightAttackSprite;
        _heavyAttackIcon.sprite = _shipWeaponHolder.HeavyAttackSprite;
    }

    private void Update()
    {
        _lightAttackPowerUpBorder.fillAmount = _shipWeaponHolder.PowerUpTimeRemaining / _shipWeaponHolder.PowerUpDuration;
    }
}
