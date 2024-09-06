using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EnemyAttack : EnemyAction
{
    [SerializeField]
    private List<Attack> _attacks = new List<Attack>();

    [SerializeField]
    private BulletLauncher _launcher;

    protected override IEnumerator ActionInstructions()
    {
        foreach(Attack attack in _attacks)
        {
            int curCount = attack.Count;
            float curSpread = attack.Spread;
            float curAngleOffset = attack.AngleOffsetStart;

            for(int i = 0; i < attack.Repetitions; i++)
            {
                _launcher.Launch(new PatternData(attack.Bullet, curCount, curSpread, curAngleOffset, attack.RandomAngleOffset, DamageTeam.Enemy, null, attack.StartAtFixedAngle ? attack.FixedAngle : null));

                curCount += attack.CountModifier;
                curSpread += attack.SpreadModifier;
                curAngleOffset += attack.AngleOffsetIncrease;

                yield return new WaitForSeconds(attack.RepeatDelay);
            }

            yield return new WaitForSeconds(attack.EndDelay);
        }
    }
}
