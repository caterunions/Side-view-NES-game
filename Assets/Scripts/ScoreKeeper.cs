using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public event Action<ScoreKeeper, int> OnScoreChange;

    [SerializeField]
    private EnemySpawner _enemySpawner;

    private int _score = 0;
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            OnScoreChange?.Invoke(this, Score - value);
        }
    }

    private void OnEnable()
    {
        Score = 0;

        _enemySpawner.OnSpawnedEnemyDeath += HandleEnemyScoreReward;
    }

    private void OnDisable()
    {
        _enemySpawner.OnSpawnedEnemyDeath -= HandleEnemyScoreReward;
    }

    private void HandleEnemyScoreReward(EnemySpawner spawner, EnemyPackage enemy, bool killedByPlayer)
    {
        if (!killedByPlayer) return;

        Score += enemy.EnemyBrain.ScoreReward;
    }
}