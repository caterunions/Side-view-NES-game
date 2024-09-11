using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollTextUp : MonoBehaviour
{
    public event Action<RollTextUp, float> OnCurrentValueChange;

    [SerializeField]
    private float _incrementBy = 1;
    public float IncrementBy
    {
        get { return _incrementBy; }
        set { _incrementBy = value; }
    }

    [SerializeField]
    private int _endValue;
    public int EndValue
    {
        get { return _endValue; }
        set { _endValue = value; }
    }

    [SerializeField]
    private int _startValue;
    public int StartValue
    {
        get { return _startValue; }
        set { _startValue = value; }
    }

    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private string _format = "";
    [SerializeField]
    private string _unit = "";

    private float _currentValue = 0;
    public float CurrentValue
    {
        get { return _currentValue; }
        set
        {
            OnCurrentValueChange?.Invoke(this, _currentValue);
            _currentValue = value;
        }
    }

    private void OnEnable()
    {
        CurrentValue = StartValue;
    }

    private void FixedUpdate()
    {
        if(CurrentValue < EndValue)
        {
            CurrentValue += IncrementBy;
            if(CurrentValue > EndValue)
            {
                CurrentValue = EndValue;
            }
            _text.text = $"{CurrentValue.ToString(_format)}{_unit}";
        }
    }
}
