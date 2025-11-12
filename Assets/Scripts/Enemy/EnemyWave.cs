using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    [SerializeField]
    private EnemySpawnData[] _enemies;
    public EnemySpawnData[] Enemies => _enemies;

    [SerializeField]
    private int _weight = 1;
    public int Weight => _weight;

    [SerializeField]
    private float _waitTimeUntilNextWave = 5;
    public float WaitTimeUntilNextWave => _waitTimeUntilNextWave;

    [SerializeField]
    private int _minScoreToSpawn = 0;
    public int MinScoreToSpawn => _minScoreToSpawn;
}

[System.Serializable]
public struct EnemySpawnData
{
    [SerializeField]
    private EnemyBrain _enemy;
    public EnemyBrain Enemy => _enemy;

    [SerializeField]
    private bool _flipSpline;
    public bool FlipSpline => _flipSpline;

    [SerializeField]
    private float _initDelay;
    public float InitDelay => _initDelay;
}