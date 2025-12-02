using System;
using UnityEngine;
using System.Collections.Generic;

using Audio.CustomSource;
using Audio.Linker;
using Audio.GUID;

namespace Audio.Subsystems
{


    [Serializable, CreateAssetMenu(
    fileName = "AudioClipEvent",
    menuName = "Audio/Audio Clip Event Rack")]
    public class AudioEventRack : ScriptableObject, IAudioSubsystem
    {
        ///////////////////AUDIO SYSTEM//////////////////////
        [SerializeField] private AudioSystemsGUID _audioSystemGUID = AudioSystemsGUID.Empty;
        public AudioSystemsGUID AudioSystemGUID
        { get { return _audioSystemGUID; } set { _audioSystemGUID = value; } }
        ////////////////////SUBSYSTEM///////////////////////
        /// TODO: CONVERT SUBSYS IDS TO AUDIOSYSTEMSGUID
        [SerializeField] private string _audioSubsystemID;
        public string AudioSubsystemID => _audioSubsystemID;
        //////////////////////////////////////////////////


        [SerializeField] private CustomAudioClip _clip;
        public CustomAudioClip Clip => _clip;

        [SerializeField]
        private bool _clipGroupMode;

        //USED ONLY IF IN GROUP MODE
        [SerializeField]
        private List<CustomAudioClip> _clips;
        public List<CustomAudioClip> Clips => _clips;


        [SerializeField]
        private List<AudioEvent> _events = new();
        public List<AudioEvent> Events => _events;

        //

        [SerializeField]
        AudioEventDispatcher _dispatcher;
        public AudioEventDispatcher Dispatcher => _dispatcher;


        private void OnEnable()
        {
            _dispatcher = new AudioEventDispatcher(this);
        }


        /// <summary>
        /// Links the audio clips to Unity's audio system
        /// </summary>
        public void LinkToUnity()
        {
            if (_clipGroupMode)
                foreach (CustomAudioClip clip in _clips)
                {
                    UnityAudioLink.InitializeClipWithEventRack(clip, _audioSystemGUID, this);
                }
            else
            {
                UnityAudioLink.InitializeClipWithEventRack(_clip, _audioSystemGUID, this);
            }
        }
        
    }
}