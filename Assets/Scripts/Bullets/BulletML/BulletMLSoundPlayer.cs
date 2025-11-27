using System.Collections.Generic;
using UnityEngine;

public class BulletMLSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _source;

    [SerializeField]
    private List<AudioClip> _sounds = new List<AudioClip>();

    public void PlaySound(string soundName)
    {
        AudioClip found = _sounds.Find(s => s.name == soundName);

        if (found == null) return;

        _source.PlayOneShot(found);
    }
}