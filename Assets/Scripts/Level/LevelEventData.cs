using UnityEngine;
using System.Collections.Generic;

public abstract class LevelEventData : ScriptableObject
{
    [SerializeField]
    private int _scoreTrigger;
    public int ScoreTrigger => _scoreTrigger;
}

[CreateAssetMenu(fileName = "Special Enemy", menuName = "Level Events/Special Enemy")]
public class SpecialEnemyEvent : LevelEventData
{
    [SerializeField]
    private List<EnemySpawnData> _enemiesToSpawn;
}