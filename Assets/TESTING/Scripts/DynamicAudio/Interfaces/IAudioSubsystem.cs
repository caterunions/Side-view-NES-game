using System;
using Audio.GUID;

namespace Audio.Subsystems
{
    public interface IAudioSubsystem
    {
        AudioSystemsGUID AudioSystemGUID { get; set; }
        AudioSystemsGUID AudioSubsystemID { get; }
    }
}
