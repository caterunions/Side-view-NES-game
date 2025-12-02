using System;

namespace Audio.GUID
{
    /// <summary>
    /// A GUID for identifying Audio Systems
    /// </summary>
    [Serializable]
    public struct AudioSystemsGUID 
    {
        ////////// static methods //////////
        public static readonly AudioSystemsGUID Empty = new AudioSystemsGUID(Guid.Empty);
        public static AudioSystemsGUID NewGuid()
        {
            return new AudioSystemsGUID(Guid.NewGuid());
        }

        //////// operators ////////
        public static bool operator ==(AudioSystemsGUID left, AudioSystemsGUID right)
        {
            return left.guid == right.guid;
        }

        public static bool operator !=(AudioSystemsGUID left, AudioSystemsGUID right)
        {
            return left.guid != right.guid;
        }

        ////////// overrides ////////
        public override readonly bool Equals(object obj)
        {
            return obj is AudioSystemsGUID other && Equals(other);
        }

        public override readonly int GetHashCode()
        {
            return guid.GetHashCode();
        }

        ////////// instance methods ////////

        public Guid guid;
        public string guidString;

        public AudioSystemsGUID(Guid g)
        {
            guid = g;
            guidString = g.ToString();
        }
    }
}
