using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Special Enemy", menuName = "Level Events/Special Enemy")]
public class SpecialEnemyEvent : LevelEventData
{
    [SerializeField]
    private List<EnemySpawnData> _enemiesToSpawn;
    public List<EnemySpawnData> EnemiesToSpawn => _enemiesToSpawn;
}