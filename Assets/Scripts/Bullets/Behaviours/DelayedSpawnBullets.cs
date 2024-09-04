using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DelayedSpawnBullets : BulletBehaviour
{
    [SerializeField]
    private float _spawnDelay;

    [SerializeField]
    private List<Attack> _attacks = new List<Attack>();

    private void OnEnable()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(_spawnDelay);

        foreach(Attack attack in _attacks)
        {
            int curCount = attack.Count;
            float curSpread = attack.Spread;
            float curAngleOffset = attack.AngleOffsetStart;

            for (int i = 0; i < attack.Repetitions; i++)
            {
                bullet.Launcher.Launch(new PatternData(attack.Bullet, curCount, curSpread, curAngleOffset, attack.RandomAngleOffset, bullet.Team, bullet.transform.position, attack.StartAtFixedAngle ? attack.FixedAngle : null));

                curCount += attack.CountModifier;
                curSpread += attack.SpreadModifier;
                curAngleOffset += attack.AngleOffsetIncrease;

                yield return new WaitForSeconds(attack.RepeatDelay);
            }

            yield return new WaitForSeconds(attack.EndDelay);
        }
    }
}
