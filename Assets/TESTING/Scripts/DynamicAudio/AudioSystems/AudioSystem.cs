using System;
using System.Collections.Generic;
using UnityEngine;

using Audio.EventRacks;


namespace Audio
{
    [Serializable]
    public abstract class AudioSystem : MonoBehaviour
    {
        [SerializeField]
        private List<AudioEventRack> _eventRacks = new();

        protected virtual AudioEventRack GetEventRackByID(string rackID)
        {
            return _eventRacks.Find(rack => rack.ID == rackID);
        }
    }
}

