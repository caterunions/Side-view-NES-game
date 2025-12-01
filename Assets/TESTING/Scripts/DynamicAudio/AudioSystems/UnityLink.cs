using System;
using System.Collections.Generic;
using Audio.SourceData;
using NUnit.Framework;
using UnityEngine;

namespace Audio.Linker
{

    public static class UnityAudioLink
    {
        internal static GameObject AudioParent;
        internal static List<AudioSystem> audioSystems = new();


        public static AudioSystem GetAudioSystem(string systemID)
        {
            return audioSystems.Find(system => system.SystemTag == systemID);
        }

        public static void Update()
        {

        }

        public static void InitializeClip(CustomAudioClip customClip)
        {
            Debug.Log("[UnityAudioLink]: Initializing Clip \"" + customClip.clip.name + "\"");
            GameObject audioSourceGameObject = new("UAC_" + customClip.clip.name);
            AudioSource unityAudioSource = audioSourceGameObject.AddComponent<AudioSource>();

            customClip.Initialize();
            customClip.ApplyToSource(unityAudioSource);
            customClip.unityInstance = unityAudioSource;

            audioSourceGameObject.transform.SetParent(AudioParent.transform);

            if (customClip.playOnAwake)
            {
                customClip.Play();
            }
        }

    }
}
