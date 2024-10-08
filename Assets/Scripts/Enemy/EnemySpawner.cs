using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public event Action<EnemySpawner, EnemyPackage, bool> OnSpawnedEnemyDeath;

    [SerializeField]
    private ScoreKeeper _scoreKeeper;

    [SerializeField]
    private List<EnemyWave> _waves;

    [SerializeField]
    private List<EnemyPackage> _aliveEnemies;

    [SerializeField]
    private float _enemyDestroyHeight;

    private float _nextSpawnTime;

    public List<Transform> EnemyTransforms
    {
        get
        {
            return _aliveEnemies.Select(e => e.EnemyBrain.transform).ToList();
        }
    }

    private float _timeRemaining
    {
        get
        {
            return _nextSpawnTime - Time.time;
        }
    }

    private void SpawnWave(EnemyWave wave)
    {
        foreach(EnemySpawnData data in wave.Enemies)
        {
            EnemyPackage enemy = Instantiate(data.EnemyPackage, new Vector3(data.SpawnPos.x, data.SpawnPos.y, 0), Quaternion.identity);
            enemy.EnemyBrain.SetPlayer(GameManager.Instance.Player.gameObject);
            enemy.EnemyBrain.Spawner = this;

            enemy.SplineAnimate.StartOffset = data.SplineStartOffset;
            if (data.FlipSpline) enemy.SplineContainer.transform.localScale = new Vector3(-1, 1, 1);

            enemy.HealthDamageReceiver.OnDamage += MonitorEnemyHealth;

            _aliveEnemies.Add(enemy);
        }

        _nextSpawnTime = Time.time + (wave.WaitTimeUntilNextWave / (1 + (_scoreKeeper.Score / 100000)));
    }

    private void DestroyEnemy(EnemyPackage enemy, bool killedByPlayer)
    {
        if(!_aliveEnemies.Contains(enemy)) return;

        enemy.HealthDamageReceiver.OnDamage -= MonitorEnemyHealth;
        OnSpawnedEnemyDeath?.Invoke(this, enemy, killedByPlayer);
        _aliveEnemies.Remove(enemy);

        Destroy(enemy.gameObject);
    }

    private void MonitorEnemyHealth(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        if (!result.Killed) return;

        EnemyPackage enemy = dr.GetComponentInParent<EnemyPackage>();

        DestroyEnemy(enemy, true);
    }

    private EnemyWave GetRandomWeightedWave(List<EnemyWave> waves)
    {
        int[] weights = waves.Where(w => _scoreKeeper.Score >= w.MinScoreToSpawn).Select(w => w.Weight).ToArray();
        int randomWeight = UnityEngine.Random.Range(0, weights.Sum());
        for (int i = 0; i < weights.Length; ++i)
        {
            randomWeight -= weights[i];
            if (randomWeight < 0)
            {
                return waves[i];
            }
        }

        return null;
    }

    private void Update()
    {
        if(_aliveEnemies.Count == 0 || _timeRemaining <= 0)
        {
            SpawnWave(GetRandomWeightedWave(_waves));
        }

        CleanupEnemies();
    }

    private void CleanupEnemies()
    {
        foreach(EnemyPackage enemy in _aliveEnemies.ToList())
        {
            if(enemy.EnemyBrain.transform.position.y <= _enemyDestroyHeight)
            {
                DestroyEnemy(enemy, false);
            }
        }
    }

    public void ForceClearEnemies()
    {
        foreach (EnemyPackage enemy in _aliveEnemies.ToList())
        {
            DestroyEnemy(enemy, false);
        }
    }
}