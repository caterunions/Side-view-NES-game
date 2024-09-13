using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBulletFromBundle : Bullet
{
    [SerializeField]
    private List<Bullet> _bullets = new List<Bullet>();

    protected override void Awake()
    {
        foreach (Bullet b in _bullets)
        {
            b.gameObject.SetActive(false);
        }
    }

    public override void Initialize(GameObject spawner, BulletLauncher launcher, DamageTeam team)
    {
        Bullet chosenBullet = _bullets[Random.Range(0, _bullets.Count)];

        _bullets.Remove(chosenBullet);

        foreach (Bullet b in _bullets)
        {
            Destroy(b.gameObject);
        }

        chosenBullet.gameObject.SetActive(true);

        chosenBullet.Initialize(spawner, launcher, team);

        chosenBullet.transform.parent = null;

        Destroy(gameObject);
    }
}
