using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private Image _overlayImage;

    [SerializeField]
    private TextMeshProUGUI _gameEndText;

    [SerializeField]
    private string _badEndMessage;

    [SerializeField]
    private string _goodEndMessage;

    [SerializeField]
    private GameObject _hiScoreContainer;

    [SerializeField]
    private TextMeshProUGUI _hiScoreText;

    [SerializeField]
    private GameObject _scoreContainer;

    [SerializeField]
    private RollTextUp _scoreRollUp;

    [SerializeField]
    private TextMeshProUGUI _newHiScoreText;

    [SerializeField]
    private Button _retryButton;

    [SerializeField]
    private Button _titleButton;

    [SerializeField]
    private float _revealDelay = 1;

    [SerializeField]
    private ScoreKeeper _scoreKeeper;

    private Coroutine _gameOverRoutine;

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnGameEnd += HandleGameOver;
        _overlayImage.gameObject.SetActive(false);
        _gameEndText.gameObject.SetActive(false);
        _hiScoreContainer.gameObject.SetActive(false);
        _scoreContainer.gameObject.SetActive(false);
        _newHiScoreText.gameObject.SetActive(false);
        _retryButton.gameObject.SetActive(false);
        _titleButton.gameObject.SetActive(false);

        _retryButton.onClick.AddListener(ReloadScene);
        _titleButton.onClick.AddListener(LoadTitle);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameEnd -= HandleGameOver;

        _retryButton.onClick.RemoveListener(ReloadScene);
        _titleButton.onClick.RemoveListener(LoadTitle);
    }

    private void Start()
    {
        OnEnable();
    }

    private void HandleGameOver(GameManager gameManager, bool goodEnd)
    {
        _gameOverRoutine = StartCoroutine(GameOverRoutine(goodEnd));
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }

    private IEnumerator GameOverRoutine(bool goodEnd)
    {
        _overlayImage.gameObject.SetActive(true);
        _gameEndText.gameObject.SetActive(true);

        if (goodEnd) _gameEndText.text = _goodEndMessage;
        else _gameEndText.text = _badEndMessage;

        yield return new WaitForSeconds(_revealDelay);

        _hiScoreContainer.SetActive(true);
        _hiScoreText.text = GameManager.Instance.HiScore.ToString("00000000");

        yield return new WaitForSeconds(_revealDelay);

        _scoreContainer.SetActive(true);
        _scoreRollUp.EndValue = _scoreKeeper.Score;

        yield return new WaitUntil(() => _scoreRollUp.CurrentValue == _scoreRollUp.EndValue);
        yield return new WaitForSeconds(_revealDelay);

        if(_scoreKeeper.Score > GameManager.Instance.HiScore)
        {
            switch(GameManager.Instance.Gamemode)
            {
                case Gamemode.Standard:
                    PlayerPrefs.SetInt("StandardHiScore", _scoreKeeper.Score);
                    break;
                case Gamemode.Endless:
                    PlayerPrefs.SetInt("EndlessHiScore", _scoreKeeper.Score);
                    break;
                default:
                    break;
            }
            PlayerPrefs.Save();
            _newHiScoreText.gameObject.SetActive(true);

            yield return new WaitForSeconds(_revealDelay);
        }

        _retryButton.gameObject.SetActive(true);
        _titleButton.gameObject.SetActive(true);
    }
}