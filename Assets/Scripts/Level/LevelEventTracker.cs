using System;
using UnityEngine;

public abstract class LevelEventTracker : MonoBehaviour
{
    public event Action<LevelEventTracker> OnEventComplete;
}
