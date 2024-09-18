using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private PlayerMenuChoices _menuChoices;

    public Gamemode Gamemode
    {
        get { return _menuChoices.Gamemode; }
    }

    [SerializeField]
    private Transform _cursor;

    [SerializeField]
    private EnemySpawner _enemySpawner;
    public EnemySpawner EnemySpawner => _enemySpawner;

    public Transform Cursor => _cursor;

    public PlayerStats Player { get; private set; }

    private void Awake()
    {
        Instance = this;
        Player = Instantiate(_menuChoices.PlayerShip, Vector3.zero, Quaternion.identity);
    }
}
