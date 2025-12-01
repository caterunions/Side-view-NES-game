using System;

namespace Audio.Subsystems
{
    interface IAudioSubsystem
    {
        public Guid AudioSystemID { get; set; }
        public string ID { get; }
    }
}
