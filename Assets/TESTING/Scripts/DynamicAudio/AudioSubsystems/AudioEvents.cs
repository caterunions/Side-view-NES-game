using System;
using Audio.CustomSource;
using UnityEngine;

public enum AudioEventType
{
    None,
    ClipBeginPlay,
    ClipPlayEnded,
    ClipTempoIncrease,
    ClipTempoDecrease,
    ClipCustomTime
    // pitch change and volume change events - if needed - can be added here in future
}

namespace Audio.Subsystems
{
    [Serializable]
    /// <summary>
    /// Class representing an audio event and its parameters
    /// </summary>
    public class AudioEvent
    {
        [SerializeField] private AudioEventType _eventType;
        public AudioEventType EventType => _eventType;

        ///////////////////EVENT PARAMETERS//////////////////////
        [SerializeField] private float _customTime = 0f;
        public float CustomTime => _customTime;
    }
}
