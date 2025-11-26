using NUnit.Framework;

using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;

public class BulletMLSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _source;

    [SerializeField]
    private List<AudioClip> _sounds = new List<AudioClip>();

    public void PlaySound(string soundName)
    {
        AudioClip found = _sounds.FirstOrDefault(s => s.name == soundName);

        if (found == null) return;

        _source.PlayOneShot(found);
    }
}