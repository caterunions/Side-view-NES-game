using System;
using System.Collections.Generic;
using UnityEngine;
using Audio.Events;

namespace Audio
{


    [Serializable]
    public abstract class AudioEventSystem : MonoBehaviour
    {
        [SerializeField]
        protected List<AudioEvents> loadedAudioEvents = new();

        protected virtual void GetClip(string clipID)
        {
            loadedAudioEvents.Find(clip => clip.GetClipID() == clipID);
        }
    }

    [Serializable]
    public class AudioSequence
    {
        [SerializeField] private List<AudioEvent> sequenceEvents = new();
        [SerializeField] private string sequenceID;
    }

    [Serializable]
    public class AudioEvent
    {
        [SerializeField] private AudioEventType eventType;
        [SerializeField] private string eventID;
        [SerializeField] private float customTime = 0f;
        private event EventHandler EventFire;


        public void Subscribe(EventHandler subscriber)
        {
            EventFire += subscriber;
        }

        public void UnSubscribe(EventHandler subscriber)
        {
            EventFire -= subscriber;
        }

        public AudioEventType GetEventType() => eventType;
        public string GetEventID() => eventID;
        public float GetEventCustomTime() => customTime;

    }

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

}

