using System;
using System.Collections.Generic;

public abstract class LevelEventTracker
{
    public event Action<LevelEventTracker> OnEventComplete;

    public bool Ongoing { get; private set; }

    protected void RaiseEventComplete(LevelEventTracker eventTracker)
    {
        Ongoing = false;
        OnEventComplete?.Invoke(eventTracker);
    }

    public void StartEvent()
    {
        Ongoing = true;
        EventInstructions();
    }

    protected abstract void EventInstructions();
}

public class SpecialEnemyEventTracker : LevelEventTracker
{
    private EnemySpawner _enemySpawner;

    private int _enemiesTracked;

    private List<EnemySpawnData> _enemiesToSpawn;

    public SpecialEnemyEventTracker(EnemySpawner enemySpawner, List<EnemySpawnData> enemiesToSpawn)
    {
        _enemySpawner = enemySpawner;
        _enemiesToSpawn = enemiesToSpawn;
    }

    protected override void EventInstructions()
    {
        _enemiesTracked = _enemiesToSpawn.Count;

        foreach (EnemySpawnData data in _enemiesToSpawn)
        {
            EnemyBrain enemy = _enemySpawner.SpawnEnemy(data);
            enemy.DamageReceiver.OnDamage += MonitorEnemyHealth;
        }
    }

    private void MonitorEnemyHealth(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        if (!result.Killed) return;

        dr.OnDamage -= MonitorEnemyHealth;

        _enemiesTracked--;

        if (_enemiesTracked <= 0)
        {
            RaiseEventComplete(this);
        }
    }
}