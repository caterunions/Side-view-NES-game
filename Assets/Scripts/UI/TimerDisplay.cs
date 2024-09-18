using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField]
    private TimeKeeper _timeKeeper;

    [SerializeField]
    private TextMeshProUGUI _timerText;

    private void Update()
    {
        if (_timeKeeper.enabled == false)
        {
            _timerText.text = "";
            return;
        }

        _timerText.text = _timeKeeper.TimeRemaining.ToString("00");
    }
}