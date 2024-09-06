using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public event Action<PlayerFire> OnBeginLightFiring;
    public event Action<PlayerFire> OnStopLightFiring;

    public event Action<PlayerFire> OnBeginHeavyFiring;
    public event Action<PlayerFire> OnStopHeavyFiring;

    [SerializeField]
    private ShipWeaponHolder _shipWeaponHolder;

    [SerializeField]
    private BulletLauncher _launcher;

    private bool _firingLight;
    public bool FiringLight => _firingLight;

    private bool _firingHeavy;
    public bool FiringHeavy => _firingHeavy;

    private Coroutine _fireRoutine = null;

    public void StartFiringLight()
    {
        _firingLight = true;
        if (_fireRoutine == null)
        {
            SwitchFiringLight();
        }
    }

    public void StopFiringLight()
    {
        _firingLight = false;
        OnStopLightFiring?.Invoke(this);
    }

    public void StartFiringHeavy()
    {
        _firingHeavy = true;
        if (_fireRoutine == null && _shipWeaponHolder.CanFireHeavyWeapon)
        {
            SwitchFiringHeavy();
        }
    }

    public void StopFiringHeavy()
    {
        _firingHeavy = false;
        OnStopHeavyFiring?.Invoke(this);
    }

    private void SwitchFiringLight()
    {
        _fireRoutine = StartCoroutine(LightFireRoutine());
        OnBeginLightFiring?.Invoke(this);
    }

    private void SwitchFiringHeavy()
    {
        _fireRoutine = StartCoroutine(HeavyFireRoutine());
        OnBeginHeavyFiring?.Invoke(this);
    }

    private IEnumerator LightFireRoutine()
    {
        while(_firingLight)
        {
            Attack attack = _shipWeaponHolder.LightAttack;

            int curCount = attack.Count;
            float curSpread = attack.Spread;
            float curAngleOffset = attack.AngleOffsetStart;

            for (int i = 0; i < attack.Repetitions; i++)
            {
                _launcher.Launch(new PatternData(attack.Bullet, curCount, curSpread, curAngleOffset, attack.RandomAngleOffset, DamageTeam.Player, null, attack.StartAtFixedAngle ? attack.FixedAngle : null));

                curCount += attack.CountModifier;
                curSpread += attack.SpreadModifier;
                curAngleOffset += attack.AngleOffsetIncrease;

                yield return new WaitForSeconds(attack.RepeatDelay);
            }

            yield return new WaitForSeconds(attack.EndDelay);

            if (_firingHeavy && _shipWeaponHolder.CanFireHeavyWeapon) break;
            if (!_firingLight) break;
        }

        if(_firingHeavy && _shipWeaponHolder.CanFireHeavyWeapon)
        {
            SwitchFiringHeavy();
        } 
        else
        {
            _fireRoutine = null;
        }
    }

    private IEnumerator HeavyFireRoutine()
    {
        while(_firingHeavy)
        {
            if (_shipWeaponHolder.ChargePercentage < 1)
            {
                _shipWeaponHolder.ChargePercentage += Time.deltaTime / _shipWeaponHolder.HeavyChargeTime;
            }

            if (!_firingHeavy) break;

            yield return new WaitForEndOfFrame();
        }

        if(_shipWeaponHolder.ChargePercentage >= 1)
        {
            _shipWeaponHolder.DeductHeavyWeaponCost();

            _shipWeaponHolder.ChargePercentage = 0;

            Attack attack = _shipWeaponHolder.HeavyAttack;

            int curCount = attack.Count;
            float curSpread = attack.Spread;
            float curAngleOffset = attack.AngleOffsetStart;

            for (int i = 0; i < attack.Repetitions; i++)
            {
                _launcher.Launch(new PatternData(attack.Bullet, curCount, curSpread, curAngleOffset, attack.RandomAngleOffset, DamageTeam.Player, null, attack.StartAtFixedAngle ? attack.FixedAngle : null));

                curCount += attack.CountModifier;
                curSpread += attack.SpreadModifier;
                curAngleOffset += attack.AngleOffsetIncrease;

                yield return new WaitForSeconds(attack.RepeatDelay);
            }

            yield return new WaitForSeconds(attack.EndDelay);
        }

        _shipWeaponHolder.ChargePercentage = 0;

        if (_firingHeavy && _shipWeaponHolder.CanFireHeavyWeapon)
        {
            SwitchFiringHeavy();
        }
        else if (_firingLight)
        {
            SwitchFiringLight();
        }
        else
        {
            _fireRoutine = null;
        }
    }
}
