using System;
using System.Collections.Generic;
using UnityEngine;

using Audio.GUID;
using Audio.Linker;
using Audio.Subsystems;


namespace Audio
{
    [Serializable]
    public abstract class AudioSystem : MonoBehaviour
    {
        public AudioSystemsGUID AudioSystemGUID = AudioSystemsGUID.Empty;
        public AudioSubsystemCollection LinkedSubsystems = new();


        private void OnEnable()
        {
            //generate new GUID
            AudioSystemGUID = AudioSystemsGUID.NewGuid();

            AudioSystems.Add(this);

            //set system ID for all subsystems
            LinkedSubsystems.LinkToAudioSystem(AudioSystemGUID);

            //VVVVVVVVVV | THIS WILL BE CHANGED LATER | VVVVVVVVVV
            UnityAudioLink.AudioParent = gameObject;

            Debug.Log("[AudioSystem]: Audio System \"" + AudioSystemGUID.GuidString + "\" Initialized");
        }

        protected virtual void Start()
        {
            //link subsystems to unity audio system
            LinkedSubsystems.LinkToUnity();
        }

        protected virtual void Update()
        {
            UnityAudioLink.Update();
        }

        protected virtual void OnDisable()
        {
            AudioSystems.Remove(this);
            Debug.Log("[AudioSystem]: Audio System \"" + AudioSystemGUID.GuidString + "\" Deinitialized");
        }
    }
}

