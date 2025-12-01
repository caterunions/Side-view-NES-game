using System;
using System.Collections;
using UnityEngine;

using Audio.Subsystems;
using Audio.Linker;

namespace Audio.CustomSource
{
    [CreateAssetMenu(
    fileName = "CustomAudioClip",
    menuName = "Audio/Custom Audio Clip")]
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


        internal AudioSource unityInstance;
        internal AudioEventRack attachedEventRack;

        private AudioSystem audioSystem;

        private bool stopRequested = false;
        private Coroutine playClipRoutine;

        public void Initialize()
        {
            audioSystem = UnityAudioLink.GetAudioSystem(attachedEventRack.AudioSystemID);
        }

        public void UpdateCustomClip()
        {
            if (unityInstance == null) return;
            ApplyToSource(unityInstance);
        }

        /// <summary>
        /// Plays the audio clip
        /// </summary>
        /// <returns>void</returns>
        public void Play()
        {
            stopRequested = false;
            double startDSP = AudioSettings.dspTime;

            unityInstance.PlayScheduled(startDSP);
            attachedEventRack.ClipBeginPlay(this);

            if (playClipRoutine != null)
            {
                //unityInstance.Stop();
                audioSystem.StopCoroutine(playClipRoutine);
            }

            playClipRoutine = audioSystem.StartCoroutine(TrackClipEnd(startDSP));
        }


        private IEnumerator TrackClipEnd(double startDSP)
        {
            while (!stopRequested)
            {
                // Track playback time in real DSP seconds
                double elapsed = (AudioSettings.dspTime - startDSP) * unityInstance.pitch;

                if (elapsed >= clip.length)
                {

                    if (loop && !stopRequested)
                    {
                        attachedEventRack.ClipPlayEnded(this);
                        Play(); // loop precisely
                    } else
                    {
                        Stop();
                    }

                        yield break; // stop coroutine
                }

                yield return null; // wait until next frame
            }
        }


        /// <summary>
        /// Plays the audio clip
        /// </summary>
        /// <returns>void</returns>
        public void Stop()
        {
            stopRequested = true;
            attachedEventRack.ClipPlayEnded(this);
            unityInstance.Stop();
            if (playClipRoutine != null)
                audioSystem.StopCoroutine(playClipRoutine);
        }

        public void ApplyToSource(AudioSource source)
        {
            if (source == null) return;

            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.mute = mute;
            source.bypassEffects = bypassEffects;
            source.bypassListenerEffects = bypassListenerEffects;
            source.bypassReverbZones = bypassReverbZones;
            source.playOnAwake = playOnAwake;


            //source.loop = loop; <-- handled by CustomAudioSource 

            source.spatialize = spatialize;
            source.rolloffMode = rolloffMode;
            source.dopplerLevel = dopplerLevel;
            source.spread = spread;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;

            source.spatialBlend = spatialBlend3D ? 1f : 0f;

            source.reverbZoneMix = reverbZoneMix;
            source.priority = priority;

            source.ignoreListenerPause = ignoreListenerPause;
            source.ignoreListenerVolume = ignoreListenerVolume;


            if (loopStartTime && source.time > 0f)
                source.time = 0f;
        }

    }
}