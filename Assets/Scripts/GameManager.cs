using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private Transform _cursor;

    [SerializeField]
    private EnemySpawner _enemySpawner;
    public EnemySpawner EnemySpawner => _enemySpawner;

    public Transform Cursor => _cursor;

    public GameObject Player { get; private set; }

    private void Awake()
    {
        Instance = this;
        Player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
    }
}
