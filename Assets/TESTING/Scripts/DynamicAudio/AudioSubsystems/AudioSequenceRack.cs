using System;
using System.Collections.Generic;
using Audio.GUID;
using Audio.Linker;
using UnityEngine;

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
        [SerializeField] private AudioSystemsGUID _audioSubsystemID = AudioSystemsGUID.Empty;
        public AudioSystemsGUID AudioSubsystemID => _audioSubsystemID;
        ////////////////////////////////////////////////////


        [SerializeField] private List<ClipSequences> _sequences = new();
        public List<ClipSequences> Sequences => _sequences;

        private void OnEnable()
        {
            _audioSubsystemID = AudioSystemsGUID.NewGuid();
            AudioSystems.Add(this);
        }

        private void OnDisable()
        {
            AudioSystems.Remove(this);
        }

    }

}