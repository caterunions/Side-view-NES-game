using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Menu Choices")]
public class PlayerMenuChoices : ScriptableObject
{
    [SerializeField]
    private PlayerStats _playerShip;
    public PlayerStats PlayerShip
    {
        get { return _playerShip; }
        set { _playerShip = value; }
    }

    [SerializeField]
    private Gamemode _gamemode;
    public Gamemode Gamemode
    {
        get { return _gamemode; }
        set { _gamemode = value; }
    }
}

public enum Gamemode
{
    Standard,
    Endless
}
