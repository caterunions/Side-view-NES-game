using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int _totalLevelScore;
    public int TotalLevelScore => _totalLevelScore;

    [SerializeField]
    [Tooltip("Enemies")]
    private List<EnemyWave> _waves;
    public List<EnemyWave> Waves => _waves;

    [SerializeField]
    [Tooltip("Level Events")]
    private List<LevelEventData> _eventData;
    public List<LevelEventData> EventData => _eventData;
}
