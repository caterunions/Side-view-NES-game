using System.Collections;
using UnityEditor;
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
        EnemyBrain.BulletMLPatternManager.StartPattern(AssetDatabase.GetAssetPath(_patternFile));
        yield return new WaitForSeconds(_duration);
        EnemyBrain.BulletMLPatternManager.StopPattern(_destroyBulletsOnFinish);
    }
}
