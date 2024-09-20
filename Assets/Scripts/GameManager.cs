using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gamemode
{
    Standard,
    Endless
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<GameManager, bool> OnGameEnd;

    public Gamemode Gamemode
    {
        get { return (Gamemode)PlayerPrefs.GetInt("GamemodeSelection", 0); }
    }

    [SerializeField]
    private ShipDictionary _shipDictionary;

    [SerializeField]
    private Transform _cursor;

    [SerializeField]
    private ScoreKeeper _scoreKeeper;

    [SerializeField]
    private EnemySpawner _enemySpawner;
    public EnemySpawner EnemySpawner => _enemySpawner;

    [SerializeField]
    private Bullet _cleanupProjectile;

    [SerializeField]
    private TimeKeeper _timeKeeper;

    public Transform Cursor => _cursor;

    public PlayerStats Player { get; private set; }

    private HealthDamageReceiver _playerHDR;
    private PlayerMove _playerMove;
    private PlayerInputHandler _playerInputHandler;

    private Coroutine _gameEndRoutine;

    private void Awake()
    {
        Instance = this;

        Player = Instantiate(_shipDictionary.Ships[PlayerPrefs.GetInt("ShipSelection", 0)], Vector3.zero, Quaternion.identity);

        _playerHDR = Player.GetComponentInChildren<HealthDamageReceiver>();
        _playerMove = Player.GetComponentInChildren<PlayerMove>();
        _playerInputHandler = Player.GetComponentInChildren<PlayerInputHandler>();

        if (Gamemode != Gamemode.Standard) _timeKeeper.enabled = false;
    }

    private void OnEnable()
    {
        _playerHDR.OnDamage += HandleGameOver;
        _timeKeeper.OnTimeRunOut += HandleTimeRunOut;
    }

    private void OnDisable()
    {
        _playerHDR.OnDamage -= HandleGameOver;
        _timeKeeper.OnTimeRunOut -= HandleTimeRunOut;
    }

    private void HandleGameOver(DamageReceiver dr, DamageEvent dmgEvent, DamageResult result)
    {
        if (!result.Killed) return;

        Player.gameObject.SetActive(false);

        _gameEndRoutine = StartCoroutine(GameEndRoutine(false));
    }

    private void HandleTimeRunOut(TimeKeeper timeKeeper)
    {
        _timeKeeper.enabled = false;

        _enemySpawner.ForceClearEnemies();

        Bullet cleanupProjectile = Instantiate(_cleanupProjectile, Player.transform.position, Quaternion.identity);

        cleanupProjectile.Initialize(gameObject, null, DamageTeam.Player);

        _gameEndRoutine = StartCoroutine(GameEndRoutine(true));
    }

    private IEnumerator GameEndRoutine(bool goodEnd)
    {
        _enemySpawner.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        _playerMove.enabled = false;
        _playerInputHandler.enabled = false;
    }
}
