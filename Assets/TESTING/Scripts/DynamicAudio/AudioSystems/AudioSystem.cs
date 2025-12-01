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
        public Guid ID = Guid.NewGuid();

        [Header("Audio Subsystems")]
        [SerializeField]
        private List<AudioEventRack> _eventRacks = new();

        [SerializeField]
        private List<AudioSequenceRack> _sequenceRacks = new();


        private void OnEnable()
        {
            UnityAudioLink.AudioSystems.Add(this);

            //set system ID for all subsystems
            _eventRacks.ForEach(rack => rack.AudioSystemID = ID);
            _sequenceRacks.ForEach(rack => rack.AudioSystemID = ID);


            UnityAudioLink.AudioParent = gameObject;

            Debug.Log("[AudioSystem]: Audio System \"" + ID + "\" Initialized");
        }

        protected virtual void Start() {
            LinkSubsystemsToUnity();      
        }

        protected virtual void Update()
        {
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

