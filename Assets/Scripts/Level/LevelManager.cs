using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner _enemySpawner;

    [SerializeField]
    private LevelData _levelData;

    private LevelEventData _nextEventData 
    {
        get
        {
            return _levelData.EventData.OrderBy(d => d.ScoreTrigger).FirstOrDefault(d => d.ScoreTrigger > _levelScore);
        }
    }

    private LevelEventTracker _curTrackedEvent;

    private float _levelScore = 0;

    private void OnEnable()
    {
        _enemySpawner.OnSpawnedEnemyDeath += HandleSpawnedEnemyDeath;
    }

    private void OnDisable()
    {
        _enemySpawner.OnSpawnedEnemyDeath -= HandleSpawnedEnemyDeath;
    }

    private void HandleSpawnedEnemyDeath(EnemySpawner enemySpawner, EnemyBrain enemy, bool killedByPlayer)
    {
        if (_enemySpawner.Paused) return;

        _levelScore += enemy.ScoreReward;

        if(_levelScore >= _nextEventData.ScoreTrigger)
        {
            _enemySpawner.TogglePaused(true);

            _curTrackedEvent = CreateEvent(_nextEventData);
            _curTrackedEvent.OnEventComplete += HandleLevelEventComplete;
        }
    }

    private void HandleLevelEventComplete(LevelEventTracker trackedEvent)
    {
        _curTrackedEvent.OnEventComplete -= HandleLevelEventComplete;

        _enemySpawner.TogglePaused(false);

        Destroy(_curTrackedEvent.gameObject);
    }

    private LevelEventTracker CreateEvent(LevelEventData data)
    {
        if(data is SpecialEnemyEvent specialEnemy)
        {
            
        }
        return null;
    }
}
