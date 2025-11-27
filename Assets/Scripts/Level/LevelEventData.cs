using UnityEngine;
using System.Collections.Generic;

public abstract class LevelEventData : ScriptableObject
{
    [SerializeField]
    private int _scoreTrigger;
    public int ScoreTrigger => _scoreTrigger;

    [SerializeField]
    private bool _waitForNoEnemies;
    public bool WaitForNoEnemies => _waitForNoEnemies;
}