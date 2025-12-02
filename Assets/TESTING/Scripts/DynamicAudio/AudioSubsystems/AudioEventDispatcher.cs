using System;

using Audio.CustomSource;

/*
 These events are dispatched by AudioEventCallers (e.g., CustomAudioClip) to notify subscribers.
 */
namespace Audio.Subsystems
{
    [Serializable]
    public class AudioEventDispatcher
    {
        private AudioEventRack parentRack;

        public event Action<CustomAudioSource> ClipBeginPlay;
        public event Action<CustomAudioSource> ClipPlayEnded;
        //public event Action<CustomAudioClip> ClipTempoIncrease;
        //public event Action<CustomAudioClip> ClipTempoDecrease;
        //public event Action<CustomAudioClip> ClipCustomTime;

        public AudioEventDispatcher(AudioEventRack rack)
        {
            parentRack = rack;
        }

        /// <summary>
        /// Dispatches an event based on the AudioEventType provided
        /// </summary>
        /// <param name="caller">The object calling the event</param>
        /// <param name="event">The type of event to call</param>
        public void DispatchEvent(IAudioEventCaller caller, AudioEventType @event)
        {
            if (parentRack.Events.Find(e => e.EventType == @event) != null)
            {
                CustomAudioSource clip = caller as CustomAudioSource;
                if (clip != null)
                {
                    InvokeEvent(@event, clip);
                }
            }
        }

        private void InvokeEvent(AudioEventType @event, CustomAudioSource clip)
        {
            switch (@event)
            {
                case AudioEventType.ClipBeginPlay:
                    ClipBeginPlay?.Invoke(clip);
                    break;
                case AudioEventType.ClipPlayEnded:
                    ClipPlayEnded?.Invoke(clip);
                    break;
                    //case AudioEventType.ClipTempoIncrease:
                    //    ClipTempoIncrease?.Invoke(clip);
                    //    break;
                    //case AudioEventType.ClipTempoDecrease:
                    //    ClipTempoDecrease?.Invoke(clip);
                    //    break;
                    //case AudioEventType.ClipCustomTime:
                    //    ClipCustomTime?.Invoke(clip);
                    //    break;
            }
        }
    }
}