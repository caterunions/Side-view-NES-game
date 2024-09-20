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
        _selectionOverlay.enabled = false;

        _backButton.onClick.AddListener(LoadMenu);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(LoadMenu);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("Title");
    }

    public void SetSelectionOverlay(RectTransform rTransform)
    {
        _selectionOverlay.enabled = true;

        _selectionOverlay.rectTransform.position = rTransform.position;

        if(_shipSelectionButtons.Contains(rTransform))
        {
            PlayerPrefs.SetInt("ShipSelection", Array.IndexOf(_shipSelectionButtons, rTransform));

            PlayerPrefs.Save();
        }
    }
}
