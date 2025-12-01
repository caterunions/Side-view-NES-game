using UnityEngine;
using Audio;

public class AudioManager : AudioSystem
{
    private void Awake()
    {
        GetEventRackByID("TestRack").Subscribe("ClipsPlayed", (clip) =>
        {
            Debug.LogWarning("Audio \"ClipsPlayed\" Event Fired: " + clip.name);
        }).Subscribe("ClipsPlayEnded", (clip) =>
        {
            if (clip.name == "IntroClip")
                GetEventRackByID("TestRack").GetClip("LoopClip").Play();

            Debug.LogWarning("Audio \"ClipsPlayEnded\" Event Fired: " + clip.name);
        });
    }


    //MUST HAVE THESE BELOW CALL BASE

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
