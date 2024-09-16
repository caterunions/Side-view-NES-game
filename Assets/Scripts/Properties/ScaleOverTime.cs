using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOverTime : MonoBehaviour
{
    [SerializeField]
    protected float _startScale;

    [SerializeField]
    protected float _endScale;

    [SerializeField]
    protected float _rampTime;

    protected float _endTime;

    private void OnEnable()
    {
        _endTime = Time.time + _rampTime;
    }

    private void Update()
    {
        float scale = Mathf.SmoothStep(_endScale, _startScale, (_endTime - Time.time) / _rampTime);

        transform.localScale = new Vector2(scale, scale);
    }
}
