using System;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event Action<LevelManager, int> OnLevelScoreChanged;

    [SerializeField]
    private EnemySpawner _enemySpawner;

    [SerializeField]
    private LevelData _levelData;
    public LevelData LevelData => _levelData;

    private LevelEventTracker _curTrackedEvent;

    private int _levelScore = 0;
    private bool _eventQueued = false;

    private void OnEnable()
    {
        _enemySpawner.Initialize(_levelData);

        _enemySpawner.OnSpawnedEnemyDeath += HandleSpawnedEnemyDeath;
    }

    private void OnDisable()
    {
        _enemySpawner.OnSpawnedEnemyDeath -= HandleSpawnedEnemyDeath;
    }

    private void HandleSpawnedEnemyDeath(EnemySpawner enemySpawner, EnemyBrain enemy, bool killedByPlayer)
    {
        if (_curTrackedEvent != null && // event queued exists
            !_curTrackedEvent.Ongoing && // event has not started
            _eventQueued && // we are waiting to start an event
            _enemySpawner.NumAliveEnemies == 0) // no enemies alive
        {
            _curTrackedEvent.StartEvent();
            _eventQueued = false;
        }

        if (_enemySpawner.Paused) return;

        LevelEventData nextEvent = _levelData.EventData.
            OrderBy(d => d.ScoreTrigger).
            FirstOrDefault(d => d.ScoreTrigger > _levelScore);

        _levelScore += enemy.ScoreReward;
        OnLevelScoreChanged?.Invoke(this, _levelScore);

        if (nextEvent != null && _levelScore >= nextEvent.ScoreTrigger)
        {
            _enemySpawner.TogglePaused(true);

            _curTrackedEvent = CreateEvent(nextEvent);

            if (!nextEvent.WaitForNoEnemies || _enemySpawner.NumAliveEnemies == 0)
            {
                _curTrackedEvent.StartEvent();
            }

            _eventQueued = nextEvent.WaitForNoEnemies;

            _curTrackedEvent.OnEventComplete += HandleLevelEventComplete;
        }
    }

    private void HandleLevelEventComplete(LevelEventTracker trackedEvent)
    {
        _curTrackedEvent.OnEventComplete -= HandleLevelEventComplete;

        _enemySpawner.TogglePaused(false);

        _curTrackedEvent = null;
    }

    private LevelEventTracker CreateEvent(LevelEventData data)
    {
        if (data is SpecialEnemyEvent specialEnemy)
        {
            return new SpecialEnemyEventTracker(_enemySpawner, specialEnemy.EnemiesToSpawn);
        }
        return null;
    }
}
