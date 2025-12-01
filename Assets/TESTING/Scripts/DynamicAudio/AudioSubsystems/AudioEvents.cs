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
    public class AudioEvent
    {
        [SerializeField] private AudioEventType _eventType;
        public AudioEventType EventType => _eventType;


        [SerializeField] private float _customTime = 0f;
        public float CustomTime => _customTime;


        public event Action<CustomAudioClip> EventFire;

        public void RunEvent(CustomAudioClip clip)
        {
            EventFire?.Invoke(clip);
        }

        public Delegate[] GetInvocationList()
        {
            return EventFire.GetInvocationList();
        }

    }
}
