using UnityEngine;
using Audio;
using Audio.Subsystems;
using Audio.CustomSource;

public class AudioManager : AudioSystem
{
    AudioEventRack eventRack;
    private void Awake()
    {
        //TODO: Remove string matching where possible
        eventRack = GetEventRackByID("TestRack");
        eventRack.ClipBeginPlay += OnClipPlay;
    }


    //MUST HAVE THESE BELOW CALL BASE

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetEventRackByID("TestRack").Clip.Play();
        }
    }

    private void OnClipPlay(CustomAudioClip clip)
    {
        Debug.LogWarning("Audio \"ClipsPlayed\" Event Fired: " + clip.name);
    }


    private void OnDestroy()
    {
        eventRack.ClipBeginPlay -= OnClipPlay;
    }
}
