using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    [System.Serializable]
    public struct EnemySpawnData
    {
        [SerializeField]
        private EnemyPackage _enemyPackage;
        public EnemyPackage EnemyPackage => _enemyPackage;

        [SerializeField]
        private Vector2 _spawnPos;
        public Vector2 SpawnPos => _spawnPos;

        [SerializeField]
        private bool _flipSpline;
        public bool FlipSpline => _flipSpline;

        [SerializeField]
        private float _splineStartOffset;
        public float SplineStartOffset => _splineStartOffset;
    }

    // workaround for unity not serializing 2d arrays/lists
    [System.Serializable]
    public struct WaveData
    {
        [SerializeField]
        private List<EnemySpawnData> _enemies;
        public List<EnemySpawnData> Enemies => _enemies;
    }

    [SerializeField]
    private List<WaveData> _waves;

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

    private void SpawnWave(WaveData wave)
    {
        foreach(EnemySpawnData data in wave.Enemies)
        {
            EnemyPackage enemy = Instantiate(data.EnemyPackage, new Vector3(data.SpawnPos.x, data.SpawnPos.y, 0), Quaternion.identity);
            enemy.EnemyBrain.SetPlayer(_player);

            enemy.SplineAnimate.StartOffset = data.SplineStartOffset;
            if (data.FlipSpline) enemy.SplineContainer.transform.localScale = new Vector3(-1, 1, 1);

            _aliveEnemies.Add(enemy);
        }
    }

    private IEnumerator EnemySpawnRoutine()
    {
        while(true)
        {
            SpawnWave(_waves[Random.Range(0, _waves.Count - 1)]);

            yield return new WaitForSeconds(_delayBetweenWaves);
        }
    }
}
