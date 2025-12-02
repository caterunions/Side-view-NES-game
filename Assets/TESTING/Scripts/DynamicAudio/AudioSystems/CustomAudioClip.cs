using System;
using System.Collections;
using UnityEngine;

using Audio.Linker;
using Audio.Subsystems;
using Audio.GUID;

namespace Audio.CustomSource
{
    [CreateAssetMenu(
    fileName = "CustomAudioClip",
    menuName = "Audio/Custom Audio Clip")]
    public class CustomAudioClip : ScriptableObject, IAudioEventCaller
    {
        [Header("Audio Clip"), SerializeField]
        private AudioClip _clip;
        public AudioClip Clip { get { return _clip; } set { _clip = value; } }

        [Header("Volume / Pitch"), Range(0f, 1f), SerializeField]
        private float _volume = 1f;
        public float Volume { get { return _volume; } set { _volume = value; } }

        [Range(-3f, 3f), SerializeField]
        private float _pitch = 1f;
        public float Pitch
        { get { return _pitch; } set { _pitch = value; } }

        [SerializeField]
        private bool _mute = false;
        public bool Mute
        { get { return _mute; } set { _mute = value; } }

        [SerializeField]
        private bool _bypassEffects = false;
        public bool BypassEffects
        { get { return _bypassEffects; } set { _bypassEffects = value; } }

        [SerializeField]
        private bool _bypassListenerEffects = false;
        public bool BypassListenerEffects
        { get { return _bypassListenerEffects; } set { _bypassListenerEffects = value; } }

        [SerializeField]
        private bool _bypassReverbZones = false;
        public bool BypassReverbZones
        { get { return _bypassReverbZones; } set { _bypassReverbZones = value; } }

        [SerializeField]
        private bool _playOnAwake = false;
        public bool PlayOnAwake
        { get { return _playOnAwake; } set { _playOnAwake = value; } }

        [SerializeField]
        private bool _loop = false;
        public bool Loop
        { get { return _loop; } set { _loop = value; } }

        [Header("3D Settings"), SerializeField]
        private bool _spatialize = false;
        public bool Spatialize
        { get { return _spatialize; } set { _spatialize = value; } }

        [SerializeField]
        private AudioRolloffMode _rolloffMode = AudioRolloffMode.Logarithmic;
        public AudioRolloffMode RolloffMode
        { get { return _rolloffMode; } set { _rolloffMode = value; } }

        [SerializeField]
        private float _dopplerLevel = 1f;
        public float DopplerLevel
        { get { return _dopplerLevel; } set { _dopplerLevel = value; } }

        [SerializeField]
        private float _spread = 0f;
        public float Spread
        { get { return _spread; } set { _spread = value; } }

        [SerializeField]
        private float _minDistance = 1f;
        public float MinDistance
        { get { return _minDistance; } set { _minDistance = value; } }

        [SerializeField]
        private float _maxDistance = 500f;
        public float MaxDistance
        { get { return _maxDistance; } set { _maxDistance = value; } }

        [SerializeField]
        private bool _spatialBlend3D = false;
        public bool SpatialBlend3D
        { get { return _spatialBlend3D; } set { _spatialBlend3D = value; } }


        [Header("Reverb / Effects"), SerializeField]
        private float _reverbZoneMix = 1f;
        public float ReverbZoneMix
        { get { return _reverbZoneMix; } set { _reverbZoneMix = value; } }

        [SerializeField]
        private int _priority = 128;
        public int Priority
        { get { return _priority; } set { _priority = value; } }

        [Header("Other"), SerializeField]
        private bool _ignoreListenerPause = false;
        public bool IgnoreListenerPause
        { get { return _ignoreListenerPause; } set { _ignoreListenerPause = value; } }

        [SerializeField]
        private bool _ignoreListenerVolume = false;
        public bool IgnoreListenerVolume
        { get { return _ignoreListenerVolume; } set { _ignoreListenerVolume = value; } }

        [SerializeField]
        private bool _loopStartTime = false;
        public bool LoopStartTime
        { get { return _loopStartTime; } set { _loopStartTime = value; } }


        private AudioSystem _audioSystem;
        private AudioSource _unitySource;
        private AudioEventRack _attachedEventRack; // <== only set when linked to an event rack


        private bool _stopRequested = false;
        private Coroutine _playClipRoutine;

        public void Initialize(AudioSystemsGUID linkToAudioSystem, AudioSource unitySource)
        {
            _audioSystem = UnityAudioLink.GetAudioSystem(linkToAudioSystem);
            _unitySource = unitySource;
        }

        public void AttachEventRack(AudioEventRack eventRack) => _attachedEventRack = eventRack;

        private void CallEvent(AudioEventType @event)
        {
            if (_attachedEventRack != null)
                _attachedEventRack.Dispatcher.DispatchEvent(this, @event);
        }

        /// <summary>
        /// Keeps the Unity AudioSource updated with any changes made to this CustomAudioClip
        /// </summary>
        public void UpdateCustomClip()
        {
            if (_unitySource == null) return;
            ApplyToSource(_unitySource);
        }

        //TODO: Move event calls to reduce coupling? [done :)]

        /// <summary>
        /// Plays the audio clip
        /// </summary>
        /// <returns>void</returns>
        public void Play()
        {
            if (!_unitySource)
            {
                Debug.LogError("[CustomAudioClip]: Cannot Play Clip \"" + name + "\" - AudioSource not initialized");
                return;
            }

            CallEvent(AudioEventType.ClipBeginPlay);

            _stopRequested = false;
            double startDSP = AudioSettings.dspTime;
            _unitySource.PlayScheduled(startDSP);

            if (_playClipRoutine != null)
            {
                _audioSystem.StopCoroutine(_playClipRoutine);
            }

            _playClipRoutine = _audioSystem.StartCoroutine(TrackClipEnd(startDSP));
        }


        private IEnumerator TrackClipEnd(double startDSP)
        {
            while (!_stopRequested)
            {
                double elapsed = (AudioSettings.dspTime - startDSP) * _unitySource.pitch;

                if (elapsed >= _clip.length)
                {

                    if (_loop && !_stopRequested)
                    {
                        CallEvent(AudioEventType.ClipPlayEnded);
                        Play();
                    }
                    else
                    {
                        Stop();
                    }

                    yield break;
                }

                yield return null;
            }
        }


        /// <summary>
        /// Stops the audio clip
        /// </summary>
        /// <returns>void</returns>
        public void Stop()
        {
            CallEvent(AudioEventType.ClipPlayEnded);

            _stopRequested = true;
            _unitySource.Stop();
            if (_playClipRoutine != null)
                _audioSystem.StopCoroutine(_playClipRoutine);
        }

        private void ApplyToSource(AudioSource source)
        {
            if (source == null) return;

            source.clip = _clip;
            source.volume = _volume;
            source.pitch = _pitch;
            source.mute = _mute;
            source.bypassEffects = _bypassEffects;
            source.bypassListenerEffects = _bypassListenerEffects;
            source.bypassReverbZones = _bypassReverbZones;
            source.playOnAwake = _playOnAwake;


            //source.loop = loop; <-- handled by CustomAudioSource 

            source.spatialize = _spatialize;
            source.rolloffMode = _rolloffMode;
            source.dopplerLevel = _dopplerLevel;
            source.spread = _spread;
            source.minDistance = _minDistance;
            source.maxDistance = _maxDistance;

            source.spatialBlend = _spatialBlend3D ? 1f : 0f;

            source.reverbZoneMix = _reverbZoneMix;
            source.priority = _priority;

            source.ignoreListenerPause = _ignoreListenerPause;
            source.ignoreListenerVolume = _ignoreListenerVolume;


            if (_loopStartTime && source.time > 0f)
                source.time = 0f;
        }

    }
}