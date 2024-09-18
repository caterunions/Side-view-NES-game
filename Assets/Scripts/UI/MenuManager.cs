using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System;

public class MenuManager : MonoBehaviour
{
    [Header("Stored Objects")]
    [SerializeField]
    private PlayerMenuChoices _playerMenuChoices;

    [SerializeField]
    private StoredScores _storedScores;

    [Header("UI Elements")]
    [SerializeField]
    private Button _startButton;

    [SerializeField]
    private Image _shipDisplay;

    [SerializeField]
    private TMP_Dropdown _gamemodeDropdown;

    [SerializeField]
    private TextMeshProUGUI _hiScoreText;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartGame);

        Sprite shipSprite = _playerMenuChoices.PlayerShip.Sprite;

        _shipDisplay.sprite = shipSprite;
        _shipDisplay.rectTransform.sizeDelta = new Vector2(shipSprite.textureRect.width * 2, shipSprite.textureRect.height * 2);

        UpdateGamemodeAndHiScore((int)_playerMenuChoices.Gamemode);

        _gamemodeDropdown.onValueChanged.AddListener(UpdateGamemodeAndHiScore);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartGame);

        _gamemodeDropdown.onValueChanged.RemoveListener(UpdateGamemodeAndHiScore);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void UpdateGamemodeAndHiScore(int selection)
    {
        Gamemode gamemode = (Gamemode)selection;

        _playerMenuChoices.Gamemode = gamemode;

        int hiScore = 0;
        _storedScores.Scores.TryGetValue(gamemode, out hiScore);

        _hiScoreText.text = hiScore.ToString("00000000");
    }
}
