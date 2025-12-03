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
        internal static AudioSource UnitySource;
        internal static event Action UnityUpdate;

        public static void Update()
        {
            //prevents massive errors on play rebuild in editor
#if UNITY_EDITOR
            if (UnityUpdate == null) return;
#endif
            UnityUpdate.Invoke();
        }

        public static void InitializeClip(CustomAudioSource customSource)
        {
            Debug.Log("[UnityAudioLink]: Initializing Clip \"" + customSource.Clip.name + "\"");
            GameObject audioSourceGameObject = new("UAC_" + customSource.Clip.name);
            AudioSource unityAudioSource = audioSourceGameObject.AddComponent<AudioSource>();

            //apply settings
            customSource.AttachUnityAudioSource(unityAudioSource);
            customSource.SyncWithUnitySource();
            UnityUpdate += customSource.SyncWithUnitySource;


            audioSourceGameObject.transform.SetParent(AudioParent.transform);

            if (customSource.PlayOnAwake)
            {
                customSource.Play();
            }
        }

    }
}
