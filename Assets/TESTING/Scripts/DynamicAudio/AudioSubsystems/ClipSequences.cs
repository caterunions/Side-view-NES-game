using System;
using UnityEngine;

using Audio.CustomSource;

namespace Audio.Subsystems {

    [Serializable]
    public class ClipSequences
    {
        [SerializeField] private CustomAudioClip _clip;
        public CustomAudioClip Clip => _clip;

        [SerializeField] private string _clipSequenceID;
        public string ClipSequenceID => _clipSequenceID;

    }
}
