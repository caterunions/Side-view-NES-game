using System;
using System.Collections.Generic;
using UnityEngine;
using Audio.Subsystems;

[Serializable]
public class AudioSequence
{
    [SerializeField] private List<AudioEvent> sequenceEvents = new();
    [SerializeField] private string sequenceID;
}
