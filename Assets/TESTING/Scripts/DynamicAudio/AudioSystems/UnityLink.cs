using System;
using System.Collections.Generic;
using UnityEngine;

using Audio.CustomSource;

namespace Audio.Linker
{

    public static class UnityAudioLink
    {
        internal static GameObject AudioParent;
        internal static List<AudioSystem> AudioSystems = new();
        internal static Action UpdateWithUnity;
        internal static AudioSource UnitySource;


        public static AudioSystem GetAudioSystem(Guid systemID)
        {
            return AudioSystems.Find(system => system.ID == systemID);
        }

        public static void Update()
        {
            UpdateWithUnity.Invoke();
        }

        public static void InitializeClip(CustomAudioClip customClip)
        {
            Debug.Log("[UnityAudioLink]: Initializing Clip \"" + customClip.clip.name + "\"");
            GameObject audioSourceGameObject = new("UAC_" + customClip.clip.name);
            AudioSource unityAudioSource = audioSourceGameObject.AddComponent<AudioSource>();

            customClip.Initialize();
            customClip.ApplyToSource(unityAudioSource);

            customClip.unityInstance = unityAudioSource;
            UpdateWithUnity += customClip.UpdateCustomClip;

            audioSourceGameObject.transform.SetParent(AudioParent.transform);

            if (customClip.playOnAwake)
            {
                customClip.Play();
            }
        }

    }
}
