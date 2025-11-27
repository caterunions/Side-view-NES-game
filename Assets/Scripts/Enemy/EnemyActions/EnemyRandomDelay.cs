using System.Collections;
using UnityEngine;

public class EnemyRandomDelay : EnemyAction
{
    [SerializeField]
    private float _minDelay;

    [SerializeField]
    private float _maxDelay;

    protected override IEnumerator ActionInstructions()
    {
        float rand = UnityEngine.Random.Range(_minDelay, _maxDelay);
        yield return new WaitForSeconds(rand);
    }
}
