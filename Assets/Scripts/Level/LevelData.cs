using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int _totalLevelScore;

    [SerializeField]
    [Tooltip("Enemies")]
    private List<EnemyWave> _waves;
}
