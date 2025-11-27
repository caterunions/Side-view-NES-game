using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public event Action<EnemySpawner, EnemyBrain, bool> OnSpawnedEnemyDeath;

    [SerializeField]
    private ScoreKeeper _scoreKeeper;

    [SerializeField]
    private BulletMLPatternManager _bulletMLPatternManager;

    private LevelData _levelData;

    private List<EnemyBrain> _aliveEnemies = new List<EnemyBrain>();
    public int NumAliveEnemies => _aliveEnemies.Count;

    private float _nextSpawnTime;

    public bool Paused { get; private set; }

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

    public void Initialize(LevelData data)
    {
        _levelData = data;
    }

    // spawns and initializes an enemy. also adds it to the list of tracked enemies.
    public EnemyBrain SpawnEnemy(EnemySpawnData data)
    {
        EnemyBrain enemy = Instantiate(data.Enemy, new Vector2(0, 50), Quaternion.identity);

        enemy.Initialize(GameManager.Instance.Player.gameObject, this, _bulletMLPatternManager, data.FlipSpline, data.InitDelay);

        enemy.DamageReceiver.OnDamage += MonitorEnemyHealth;

        _aliveEnemies.Add(enemy);

        return enemy;
    }

    private void SpawnWave(EnemyWave wave)
    {
        foreach (EnemySpawnData data in wave.Enemies)
        {
            SpawnEnemy(data);
        }

        _nextSpawnTime = Time.time + (wave.WaitTimeUntilNextWave / (1 + (_scoreKeeper.Score / 100000)));
    }

    public void DestroyEnemy(EnemyBrain enemy, bool killedByPlayer)
    {
        if (!_aliveEnemies.Contains(enemy)) return;

        enemy.DamageReceiver.OnDamage -= MonitorEnemyHealth;

        _aliveEnemies.Remove(enemy);

        OnSpawnedEnemyDeath?.Invoke(this, enemy, killedByPlayer);

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
        int[] weights = waves.Select(w => w.Weight).ToArray();
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
        if (Paused || _levelData == null) return;

        if (_aliveEnemies.Count == 0 || _timeRemaining <= 0)
        {
            SpawnWave(GetRandomWeightedWave(_levelData.Waves));
        }
    }

    public void ForceClearEnemies()
    {
        foreach (EnemyBrain enemy in _aliveEnemies.ToList())
        {
            DestroyEnemy(enemy, false);
        }
    }

    public void TogglePaused(bool pause)
    {
        Paused = pause;
    }
}