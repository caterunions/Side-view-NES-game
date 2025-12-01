using System;
using System.Collections.Generic;
using UnityEngine;

using Audio.Subsystems;
using Audio.Linker;


namespace Audio
{
    [Serializable]
    public abstract class AudioSystem : MonoBehaviour
    {
        public string SystemTag = "AudioSystem";

        [Header("Audio Subsystems")]
        [SerializeField]
        private List<AudioEventRack> _eventRacks = new();

        [SerializeField]
        private List<AudioSequenceRack> _sequenceRacks = new();


        private void OnEnable()
        {
            UnityAudioLink.AudioSystems.Add(this);

            //set system ID for all subsystems
            _eventRacks.ForEach(rack => rack.AudioSystemID = SystemTag);
            _sequenceRacks.ForEach(rack => rack.AudioSystemID = SystemTag);

            if (runInEditMode) return;

            if (GameObject.FindGameObjectsWithTag(SystemTag).Length > 1)
            {
                Debug.LogError("[AudioSystem]: Error, one or more GameObjects\nare using a prohibited AudioSystem tag => " + SystemTag);
                this.enabled = false;
                return;
            }

            UnityAudioLink.AudioParent = GameObject.FindGameObjectWithTag(SystemTag);
            Debug.Log("[AudioSystem]: Audio System \"" + SystemTag + "\" Initializing");
        }

        protected virtual void Start() {
            if (runInEditMode) return;
            LinkSubsystemsToUnity();      
        }

        protected virtual void Update()
        {
            if (runInEditMode) return;
            UnityAudioLink.Update();
        }


        protected virtual void LinkSubsystemsToUnity()
        {
            foreach (AudioEventRack rack in _eventRacks)
                rack.LinkToUnity();
            //add more for other substysem below (like sequence)
        }

        protected virtual AudioEventRack GetEventRackByID(string rackID)
        {
            return _eventRacks.Find(rack => rack.ID == rackID);
        }
    }
}

