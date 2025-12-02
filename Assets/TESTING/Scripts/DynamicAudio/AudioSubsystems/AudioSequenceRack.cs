using System;
using UnityEngine;
using System.Collections.Generic;

using Audio.GUID;

namespace Audio.Subsystems
{


    [Serializable, CreateAssetMenu(
    fileName = "AudioSequence",
    menuName = "Audio/Audio Clip Sequence Rack")]
    public class AudioSequenceRack : ScriptableObject, IAudioSubsystem
    {
        ///////////////////AUDIO SYSTEM//////////////////////
        [SerializeField] private AudioSystemsGUID _audioSystemGUID = AudioSystemsGUID.Empty;
        public AudioSystemsGUID AudioSystemGUID
        { get { return _audioSystemGUID; } set { _audioSystemGUID = value; } }
        ////////////////////SUBSYSTEM///////////////////////
        [SerializeField] private string _audioSubsystemID;
        public string AudioSubsystemID => _audioSubsystemID;
        ////////////////////////////////////////////////////


        [SerializeField] private List<ClipSequences> _sequences = new();
        public List<ClipSequences> Sequences => _sequences;
    }

}