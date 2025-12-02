using UnityEngine;
using Audio;
using Audio.Subsystems;
using Audio.CustomSource;

public class AudioManager : AudioSystem
{
    AudioEventRack eventRack;
    private void Awake()
    {
        //TODO: Remove string matching where possible [done :)]
        eventRack = LinkedSubsystems.EventRacks[0];
        eventRack.Dispatcher.ClipBeginPlay += OnClipPlay;
        eventRack.Dispatcher.ClipPlayEnded += OnClipEnd;
    }


    //MUST HAVE THESE BELOW CALL BASE

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.W))
        {
            eventRack.Clip.Play();
        }
    }

    private void OnClipPlay(CustomAudioClip clip)
    {
        Debug.LogWarning("Audio \"PLAY\" Event Fired: " + clip.name);
    }

    private void OnClipEnd(CustomAudioClip clip)
    {
        Debug.LogWarning("Audio \"END\" Event Fired: " + clip.name);
        if (clip.name == "IntroClip")
            eventRack.Clips[1].Play();
    }


    private void OnDestroy()
    {
        eventRack.Dispatcher.ClipBeginPlay -= OnClipPlay;
        eventRack.Dispatcher.ClipPlayEnded -= OnClipEnd;
    }
}
