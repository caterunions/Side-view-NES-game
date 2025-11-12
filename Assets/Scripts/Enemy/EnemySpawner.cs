using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public event Action<EnemySpawner, EnemyBrain, bool> OnSpawnedEnemyDeath;

    [SerializeField]
    private ScoreKeeper _scoreKeeper;

    [SerializeField]
    private List<EnemyWave> _waves;

    private List<EnemyBrain> _aliveEnemies = new List<EnemyBrain>();

    private float _nextSpawnTime;

    public List<Transform> EnemyTransforms
    {
        get
        {
            return _aliveEnemies.Select(e => e.transform).ToList();
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
            EnemyBrain enemy = Instantiate(data.Enemy, new Vector2(0, 50), Quaternion.identity);

            enemy.Initialize(GameManager.Instance.Player.gameObject, this, data.FlipSpline, data.InitDelay);

            enemy.DamageReceiver.OnDamage += MonitorEnemyHealth;

            _aliveEnemies.Add(enemy);
        }

        _nextSpawnTime = Time.time + (wave.WaitTimeUntilNextWave / (1 + (_scoreKeeper.Score / 100000)));
    }

    public void DestroyEnemy(EnemyBrain enemy, bool killedByPlayer)
    {
        if(!_aliveEnemies.Contains(enemy)) return;

        enemy.DamageReceiver.OnDamage -= MonitorEnemyHealth;
        OnSpawnedEnemyDeath?.Invoke(this, enemy, killedByPlayer);
        _aliveEnemies.Remove(enemy);

        Destroy(enemy.gameObject);
    }

    private void MonitorEnemyHealth(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        if (!result.Killed) return;

        EnemyBrain enemy = dr.GetComponentInParent<EnemyBrain>();

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
    }

    public void ForceClearEnemies()
    {
        foreach (EnemyBrain enemy in _aliveEnemies.ToList())
        {
            DestroyEnemy(enemy, false);
        }
    }
}