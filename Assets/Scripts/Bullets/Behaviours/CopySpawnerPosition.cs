using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CopySpawnerTransform : BulletBehaviour
{
    private void Update()
    {
        transform.position = bullet.Launcher.transform.position;
    }
}
