using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stored Scores")]
public class StoredScores : ScriptableObject
{
    public Dictionary<Gamemode, int> Scores { get; set; }
}
