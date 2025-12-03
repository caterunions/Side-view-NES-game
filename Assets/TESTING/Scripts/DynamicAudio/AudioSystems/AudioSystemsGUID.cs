using System;
using UnityEngine;

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
            return left.Guid == right.Guid;
        }

        public static bool operator !=(AudioSystemsGUID left, AudioSystemsGUID right)
        {
            return left.Guid != right.Guid;
        }

        ////////// overrides ////////
        public override readonly bool Equals(object obj)
        {
            return obj is AudioSystemsGUID other && Equals(other);
        }

        public override readonly int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        ////////// instance methods ////////

        [SerializeField]
        private Guid _guid;
        public readonly Guid Guid => _guid;

        [SerializeField]
        private string _guidString;
        public readonly string GuidString => _guidString;

        public AudioSystemsGUID(Guid g)
        {
            _guid = g;
            _guidString = g.ToString();
        }
    }
}
