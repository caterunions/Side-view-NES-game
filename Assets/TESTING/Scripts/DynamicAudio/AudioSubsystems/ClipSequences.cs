using System;
using UnityEngine;

using Audio.CustomSource;

namespace Audio.Subsystems {

    [Serializable]
    public class ClipSequences
    {
        [SerializeField] private CustomAudioSource _clip;
        public CustomAudioSource Clip => _clip;

        [SerializeField] private string _clipSequenceID;
        public string ClipSequenceID => _clipSequenceID;

    }
}
