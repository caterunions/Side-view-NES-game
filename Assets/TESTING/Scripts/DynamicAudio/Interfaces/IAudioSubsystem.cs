using Audio.GUID;

namespace Audio.Subsystems
{
    interface IAudioSubsystem
    {
        AudioSystemsGUID AudioSystemGUID { get; set; }
        string AudioSubsystemID { get; }
    }
}
