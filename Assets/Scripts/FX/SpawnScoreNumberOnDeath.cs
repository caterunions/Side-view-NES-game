using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScoreNumberOnDeath : MonoBehaviour
{
    private DamageReceiver _dr;
    protected DamageReceiver Dr
    {
        get
        {
            if (_dr == null) _dr = GetComponentInParent<DamageReceiver>();
            return _dr;
        }
    }

    private EnemyBrain _brain;
    protected EnemyBrain Brain
    {
        get
        {
            if (_brain == null) _brain = GetComponentInParent<EnemyBrain>();
            return _brain;
        }
    }

    [SerializeField]
    private FloatingText _textObj;

    private void OnEnable()
    {
        Dr.OnDamage += SpawnScoreNumber;
    }

    private void OnDisable()
    {
        Dr.OnDamage -= SpawnScoreNumber;
    }

    private void SpawnScoreNumber(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        if (!result.Killed) return;

        Vector2 spawnPos = transform.position;

        FloatingText scoreText = Instantiate(_textObj, spawnPos, Quaternion.identity);

        scoreText.Text.text = $"{Brain.ScoreReward}";
    }
}
