using System;
using System.Collections.Generic;
using Audio.CustomSource;
using Audio.GUID;
using Audio.Subsystems;
using UnityEngine;

namespace Audio.Linker
{
    //TODO: Rework audio spawning in unity to use only one audio source and PlayOneShot for better performance
    public static class UnityAudioLink
    {
        internal static GameObject AudioParent;
        internal static List<AudioSystem> AudioSystems = new();
        internal static AudioSource UnitySource;

        internal static event Action UnityUpdate;

        public static AudioSystem GetAudioSystem(AudioSystemsGUID systemID)
        {
            return AudioSystems.Find(system => system.AudioSystemGUID == systemID);
        }

        public static void Update()
        {
            //prevents massive errors on play rebuild in editor
#if UNITY_EDITOR
            if (UnityUpdate == null) return;
#endif
            UnityUpdate.Invoke();
        }
        //TODO: AudioEventRack parameter to be changed to AudioSystemsGUID later
        public static void InitializeClipWithEventRack(CustomAudioSource customClip, AudioSystemsGUID linkToSystem, AudioEventRack rack)
        {
            Debug.Log("[UnityAudioLink]: Initializing Clip \"" + customClip.Clip.name + "\"");
            GameObject audioSourceGameObject = new("UAC_" + customClip.Clip.name);
            AudioSource unityAudioSource = audioSourceGameObject.AddComponent<AudioSource>();

            //apply settings
            customClip.Initialize(linkToSystem, unityAudioSource);
            customClip.AttachEventRack(rack);
            customClip.UpdateCustomClip();
            UnityUpdate += customClip.UpdateCustomClip;


            audioSourceGameObject.transform.SetParent(AudioParent.transform);

            if (customClip.PlayOnAwake)
            {
                customClip.Play();
            }
        }

    }
}
