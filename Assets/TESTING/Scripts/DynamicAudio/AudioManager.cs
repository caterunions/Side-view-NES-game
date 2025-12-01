using UnityEngine;
using Audio;

public class AudioManager : AudioSystem
{
    void Awake()
    {
        GetEventRackByID("TestRack").Subscribe("ClipsPlayed", (clip) =>
        {
            Debug.Log("Audio \"ClipsPlayed\" Event Fired: " + clip.name);
        });
    }
}
