using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private EnemyWave[] _waves;

    [SerializeField]
    private List<EnemyPackage> _aliveEnemies;

    [SerializeField]
    private float _enemyDestroyHeight;

    [SerializeField]
    private float _delayBetweenWaves;

    private Coroutine _enemySpawnRoutine = null;

    private void OnEnable()
    {
        _enemySpawnRoutine = StartCoroutine(EnemySpawnRoutine());
    }

    private void OnDisable()
    {
        _enemySpawnRoutine = null;
    }

    private void SpawnWave(EnemyWave wave)
    {
        foreach(EnemySpawnData data in wave.Enemies)
        {
            EnemyPackage enemy = Instantiate(data.EnemyPackage, new Vector3(data.SpawnPos.x, data.SpawnPos.y, 0), Quaternion.identity);
            enemy.EnemyBrain.SetPlayer(_player);

            enemy.SplineAnimate.StartOffset = data.SplineStartOffset;
            if (data.FlipSpline) enemy.SplineContainer.transform.localScale = new Vector3(-1, 1, 1);

            enemy.HealthDamageReceiver.OnDamage += MonitorEnemyHealth;

            _aliveEnemies.Add(enemy);
        }
    }

    private void DestroyEnemy(EnemyPackage enemy)
    {
        if(!_aliveEnemies.Contains(enemy)) return;

        enemy.HealthDamageReceiver.OnDamage -= MonitorEnemyHealth;

        _aliveEnemies.Remove(enemy);

        Destroy(enemy.gameObject);
    }

    private void MonitorEnemyHealth(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        if (!result.Killed) return;

        EnemyPackage enemy = dr.GetComponentInParent<EnemyPackage>();

        DestroyEnemy(enemy);
    }

    private IEnumerator EnemySpawnRoutine()
    {
        while(true)
        {
            CleanupEnemies();
            SpawnWave(_waves[Random.Range(0, _waves.Length - 1)]);

            yield return new WaitForSeconds(_delayBetweenWaves);
        }
    }

    private void CleanupEnemies()
    {
        foreach(EnemyPackage enemy in _aliveEnemies.ToList())
        {
            if(enemy.EnemyBrain.transform.position.y <= _enemyDestroyHeight)
            {
                DestroyEnemy(enemy);
            }
        }
    }
}