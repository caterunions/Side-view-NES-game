using System.Collections.Generic;
using UnityEngine;

using Audio.GUID;
using Audio.Subsystems;

namespace Audio
{
    public static class AudioSystems
    {
        private static List<AudioSystem> _globalAudioSystems = new();
        public static List<AudioSystem> GlobalAudioSystems => _globalAudioSystems;

        private static List<IAudioSubsystem> _globalAudioSubsystems = new();
        public static List<IAudioSubsystem> GlobalAudioSubsystems => _globalAudioSubsystems;

        public static void Add(AudioSystem audioSystem)
        {
            _globalAudioSystems.Add(audioSystem);
        }
        public static void Add(IAudioSubsystem audioSubsystem)
        {
            _globalAudioSubsystems.Add(audioSubsystem);
        }

        public static void Remove(AudioSystem audioSystem)
        {
            _globalAudioSystems.Remove(audioSystem);
        }
        public static void Remove(IAudioSubsystem audioSubsystem)
        {
            _globalAudioSubsystems.Remove(audioSubsystem);
        }


        /// <summary>
        /// Gets an audio system by its GUID
        /// </summary>
        public static AudioSystem GetAudioSystem(AudioSystemsGUID systemGUID)
        {
            return _globalAudioSystems.Find(system => system.AudioSystemGUID == systemGUID);
        }

        /// <summary>
        /// Gets an audio subsystem by its GUID
        /// </summary>
        public static IAudioSubsystem GetAudioSubsystem(AudioSystemsGUID subsystemGUID)
        {
            return _globalAudioSubsystems.Find(subsystem => subsystem.AudioSubsystemID == subsystemGUID);
        }
    }
}