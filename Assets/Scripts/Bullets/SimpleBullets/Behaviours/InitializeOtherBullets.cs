using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeOtherBullets : BulletBehaviour
{
    [SerializeField]
    private List<Bullet> _bullets = new List<Bullet>();

    private void OnEnable()
    {
        foreach (Bullet b in _bullets)
        {
            b.gameObject.SetActive(true);

            b.Initialize(bullet.Spawner, bullet.Launcher, bullet.Team);
        }
    }
}
