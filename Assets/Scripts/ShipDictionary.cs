using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship Dictionary")]
public class ShipDictionary : ScriptableObject
{
    [SerializeField]
    private PlayerStats[] _ships = new PlayerStats[0];
    public PlayerStats[] Ships => _ships;
}
