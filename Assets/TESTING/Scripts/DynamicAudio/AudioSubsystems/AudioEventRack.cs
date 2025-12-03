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
        [SerializeField] private AudioSystemsGUID _audioSubsystemID = AudioSystemsGUID.Empty;
        public AudioSystemsGUID AudioSubsystemID => _audioSubsystemID;
        //////////////////////////////////////////////////


        [SerializeField] private CustomAudioSource _clip;
        public CustomAudioSource Clip => _clip;

        [SerializeField]
        private bool _clipGroupMode;

        //USED ONLY IF IN GROUP MODE
        [SerializeField]
        private List<CustomAudioSource> _clips;
        public List<CustomAudioSource> Clips => _clips;


        [SerializeField]
        private List<AudioEvent> _events = new();
        public List<AudioEvent> Events => _events;

        //

        [SerializeField]
        AudioEventDispatcher _dispatcher;
        public AudioEventDispatcher Dispatcher => _dispatcher;


        private void OnEnable()
        {
            _audioSubsystemID = AudioSystemsGUID.NewGuid();
            _dispatcher = new AudioEventDispatcher(this);

            //add to subsystem list
            AudioSystems.Add(this);
        }

        private void OnDisable()
        {
            AudioSystems.Remove(this);
        }


        /// <summary>
        /// Links the audio clips to Unity's audio system
        /// </summary>
        public void LinkToUnity()
        {
            if (_clipGroupMode)
                foreach (CustomAudioSource clip in _clips)
                {
                    clip.AttachAudioSystem(_audioSystemGUID);
                    clip.AttachSubsystem(_audioSubsystemID);
                    UnityAudioLink.InitializeClip(clip);
                }
            else
            {
                _clip.AttachAudioSystem(_audioSystemGUID);
                _clip.AttachSubsystem(_audioSubsystemID);
                UnityAudioLink.InitializeClip(_clip);
            }
        }
        
    }
}