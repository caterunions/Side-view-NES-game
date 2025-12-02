using System;
using System.Collections.Generic;

using Audio.GUID;
using Audio.Subsystems;

[Serializable]
public class AudioSubsystemCollection
{
    public List<AudioEventRack> EventRacks;
    public List<AudioSequenceRack> SequenceRacks;

    /// <summary>
    /// Links the subsystems to the given audio system via GUID
    /// </summary>
    /// <param name="audioSystemGUID">GUID of audio system to link to</param>
    public void LinkToAudioSystem(AudioSystemsGUID audioSystemGUID)
    {
        EventRacks.ForEach(rack => rack.AudioSystemGUID = audioSystemGUID);
        SequenceRacks.ForEach(rack => rack.AudioSystemGUID = audioSystemGUID);
    }

    /// <summary>
    /// Links the subsystems to Unity's audio system
    /// </summary>
    public void LinkToUnity()
    {
        EventRacks.ForEach(rack => rack.LinkToUnity());
        //SequenceRacks.ForEach(rack => rack.LinkToUnity()); // <- IN THE FUTURE*
    }
}
