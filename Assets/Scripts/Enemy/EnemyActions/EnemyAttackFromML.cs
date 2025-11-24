using System.Collections;
using UnityEngine;

public class EnemyAttackFromML : EnemyAction
{
    [SerializeField]
    private TextAsset _patternFile;

    [SerializeField]
    private float _duration;

    [SerializeField]
    private bool _destroyBulletsOnFinish = false;

    protected override IEnumerator ActionInstructions()
    {
        EnemyBrain.BulletMLPatternManager.StartPattern(_patternFile);
        yield return new WaitForSeconds(_duration);
        EnemyBrain.BulletMLPatternManager.StopPattern(_destroyBulletsOnFinish);
    }
}
