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
    [Header("UI Elements")]
    [SerializeField]
    private Button _startButton;

    [SerializeField]
    private Button _shipSelectButton;

    [SerializeField]
    private Image _shipDisplay;

    [SerializeField]
    private TMP_Dropdown _gamemodeDropdown;

    [SerializeField]
    private TextMeshProUGUI _hiScoreText;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartGame);

        _shipSelectButton.onClick.AddListener(LoadShipSelect);

        //Sprite shipSprite = _playerMenuChoices.PlayerShip.Sprite;

        //_shipDisplay.sprite = shipSprite;
        //_shipDisplay.rectTransform.sizeDelta = new Vector2(shipSprite.textureRect.width * 2, shipSprite.textureRect.height * 2);

        UpdateGamemodeAndHiScore(PlayerPrefs.GetInt("GamemodeSelection", 0));

        _gamemodeDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("GamemodeSelection", 0));
        _gamemodeDropdown.onValueChanged.AddListener(UpdateGamemodeAndHiScore);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartGame);

        _shipSelectButton.onClick.RemoveListener(LoadShipSelect);

        _gamemodeDropdown.onValueChanged.RemoveListener(UpdateGamemodeAndHiScore);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void LoadShipSelect()
    {
        SceneManager.LoadScene("ShipSelection");
    }

    private void UpdateGamemodeAndHiScore(int selection)
    {
        PlayerPrefs.SetInt("GamemodeSelection", selection);

        PlayerPrefs.Save();

        int hiScore = 0;
        

        _hiScoreText.text = hiScore.ToString("00000000");
    }
}
