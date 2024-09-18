using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipSelectManager : MonoBehaviour
{
    [SerializeField]
    private Button _backButton;

    [SerializeField]
    private RectTransform[] _shipSelectionButtons;

    [SerializeField]
    private Image _selectionOverlay;

    private bool _firstFramePassed = false;
    private bool _secondFramePassed = false;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(LoadMenu);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(LoadMenu);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetSelectionOverlay(RectTransform rTransform)
    {
        _selectionOverlay.rectTransform.position = rTransform.position;

        if(_shipSelectionButtons.Contains(rTransform))
        {
            PlayerPrefs.SetInt("ShipSelection", Array.IndexOf(_shipSelectionButtons, rTransform));

            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        // layout groups are fussy and need a frame to set things up
        if (_firstFramePassed && !_secondFramePassed)
        {
            SetSelectionOverlay(_shipSelectionButtons[PlayerPrefs.GetInt("ShipSelection", 0)]);
            _secondFramePassed = true;
        }
        _firstFramePassed = true;
    }
}
