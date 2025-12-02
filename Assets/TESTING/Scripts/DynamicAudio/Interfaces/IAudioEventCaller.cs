using UnityEngine;

namespace Audio.Subsystems
{
    public interface IAudioEventCaller
    {
        AudioClip Clip { get; set; }
        void CallEvent(AudioEventType @event) { }
    }
}
