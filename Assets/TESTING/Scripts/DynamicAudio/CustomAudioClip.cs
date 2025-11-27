using UnityEngine;

namespace Audio.SourceData
{
    [CreateAssetMenu(fileName = "CustomAudioClip", menuName = "Audio/Custom Audio Clip")]
    public class CustomAudioClip : ScriptableObject
    {
        [Header("Audio Clip")]
        public AudioClip clip;

        [Header("Volume / Pitch")]
        [Range(0f, 1f)] public float volume = 1f;
        [Range(-3f, 3f)] public float pitch = 1f;
        public bool mute = false;
        public bool bypassEffects = false;
        public bool bypassListenerEffects = false;
        public bool bypassReverbZones = false;
        public bool playOnAwake = false;
        public bool loop = false;

        [Header("3D Settings")]
        public bool spatialize = false;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        public float dopplerLevel = 1f;
        public float spread = 0f;
        public float minDistance = 1f;
        public float maxDistance = 500f;
        public bool spatialBlend3D = false;

        [Header("Reverb / Effects")]
        public float reverbZoneMix = 1f;
        public int priority = 128;

        [Header("Other")]
        public bool ignoreListenerPause = false;
        public bool ignoreListenerVolume = false;
        public bool loopStartTime = false;
    }
}