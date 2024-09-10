using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    [SerializeField]
    private EnemySpawnData[] _enemies;
    public EnemySpawnData[] Enemies => _enemies;
}

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