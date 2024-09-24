using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _backgroundObjects;

    [SerializeField]
    private float _minSpawnTime;

    [SerializeField]
    private float _maxSpawnTime;

    [SerializeField]
    private float _cleanupPlaneY;

    [SerializeField]
    private float _spawnRangeX;

    private List<GameObject> _spawnedObjects = new List<GameObject>();

    private Coroutine _spawnRoutine = null;

    private void OnEnable()
    {
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while(true)
        {
            Vector3 spawnPos = new Vector3(transform.position.x + Random.Range(-_spawnRangeX, _spawnRangeX), transform.position.y, 0);

            GameObject obj = Instantiate(_backgroundObjects[Random.Range(0, _backgroundObjects.Count)], spawnPos, Quaternion.identity, transform.root);
            _spawnedObjects.Add(obj);

            foreach (GameObject gObj in _spawnedObjects.ToList())
            {
                if (gObj.transform.position.y <= _cleanupPlaneY)
                {
                    _spawnedObjects.Remove(gObj);
                    Destroy(gObj);
                }
            }

            yield return new WaitForSeconds(Random.Range(_minSpawnTime, _maxSpawnTime));
        }
    }
}
