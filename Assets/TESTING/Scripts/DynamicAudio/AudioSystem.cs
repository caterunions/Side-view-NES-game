using System;
using System.Collections.Generic;
using UnityEngine;
using Audio.EventRacks;

namespace Audio
{


    [Serializable]
    public abstract class AudioEventSystem : MonoBehaviour
    {
        [SerializeField]
        protected List<AudioEventRack> loadedEventRacks = new();

        protected virtual void GetClipFromRacks(string clipID)
        {
            loadedEventRacks.Find(clip => clip.GetClipID() == clipID);
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
        [SerializeField] private AudioEventType _eventType;
        [SerializeField] private string _eventID;
        [SerializeField] private float _customTime = 0f;
        [SerializeField] private event EventHandler EventFire;


        public void Subscribe(EventHandler subscriber) => EventFire += subscriber;
        public void UnSubscribe(EventHandler subscriber) => EventFire -= subscriber;
        public string GetEventID() => _eventID;
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

