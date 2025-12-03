using UnityEngine;
using Audio;
using Audio.Subsystems;
using Audio.CustomSource;

public class AudioManager2 : AudioSystem
{
    AudioEventRack eventRack;
    private void Awake()
    {
        eventRack = LinkedSubsystems.EventRacks[0];
        eventRack.Dispatcher.ClipBeginPlay += OnClipPlay;
        eventRack.Dispatcher.ClipPlayEnded += OnClipEnd;
    }


    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.W))
        {
            eventRack.Clips[0].Play();
        }
    }

    private void OnClipPlay(CustomAudioSource clip)
    {
        Debug.LogWarning("Audio \"PLAY\" Event Fired: " + clip.name);
    }

    private void OnClipEnd(CustomAudioSource clip)
    {
        Debug.LogWarning("Audio \"END\" Event Fired: " + clip.name);
        if (clip.name == "Intro")
            eventRack.Clips[1].Play();
    }


    private void OnDestroy()
    {
        eventRack.Dispatcher.ClipBeginPlay -= OnClipPlay;
        eventRack.Dispatcher.ClipPlayEnded -= OnClipEnd;
    }
}
