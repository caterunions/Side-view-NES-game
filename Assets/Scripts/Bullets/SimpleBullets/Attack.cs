using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    [SerializeField]
    private Bullet _bullet;
    public Bullet Bullet => _bullet;

    [SerializeField]
    private int _count;
    public int Count => _count;

    [SerializeField]
    private int _countModifier;
    public int CountModifier => _countModifier;

    [SerializeField]
    private float _spread;
    public float Spread => _spread;

    [SerializeField]
    private float _spreadModifier;
    public float SpreadModifier => _spreadModifier;

    [SerializeField]
    private float _angleOffsetStart;
    public float AngleOffsetStart => _angleOffsetStart;

    [SerializeField]
    private float _angleOffsetIncrease;
    public float AngleOffsetIncrease => _angleOffsetIncrease;

    [SerializeField]
    private int _repetitions = 1;
    public int Repetitions => _repetitions;

    [SerializeField]
    private float _repeatDelay;
    public float RepeatDelay => _repeatDelay;

    [SerializeField]
    private float _endDelay;
    public float EndDelay => _endDelay;

    [SerializeField]
    private bool _startAtFixedAngle;
    public bool StartAtFixedAngle => _startAtFixedAngle;

    [SerializeField]
    private float _fixedAngle;
    public float FixedAngle => _fixedAngle;

    [SerializeField]
    private float _randomAngleOffset;
    public float RandomAngleOffset => _randomAngleOffset;
}
