using System;
using System.Collections.Generic;
using Audio;
using Audio.Subsystems;
using UnityEngine;

[Serializable, CreateAssetMenu(
fileName = "AudioSequence",
menuName = "Audio/Audio Clip Sequence")]
public class AudioSequence : ScriptableObject, IAudioSubsystem
{
    [SerializeField] private string _audioSystemID = "Not Linked To Audio System";
    public string AudioSystemID { get { return _audioSystemID; } set { _audioSystemID = value; } }

    [SerializeField] private List<AudioEvent> sequenceEvents = new();
    [SerializeField] private string sequenceID;
}
