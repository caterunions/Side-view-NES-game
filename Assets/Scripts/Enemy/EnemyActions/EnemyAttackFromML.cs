using System;
using System.Collections;
using UnityEngine;

public class EnemyAttackFromML : EnemyAction
{
    [SerializeField]
    private TextAsset _patternFile;

    [SerializeField]
    private float _duration;

    [SerializeField]
    private bool _destroyBulletsOnEnd = true;

    private Guid patternGuid;

    protected override IEnumerator ActionInstructions()
    {
        patternGuid = EnemyBrain.BulletMLPatternManager.StartPattern(_patternFile);
        yield return new WaitForSeconds(_duration);
        EnemyBrain.BulletMLPatternManager.StopPattern(patternGuid, _destroyBulletsOnEnd);
    }

    protected override void ExtraStopInstructions()
    {
        EnemyBrain.BulletMLPatternManager.StopPattern(patternGuid, _destroyBulletsOnEnd);
    }
}
