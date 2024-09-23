using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyBulletLauncher : MonoBehaviour
{
    [SerializeField]
    private BulletLauncher _launcherToCopy;

    private void OnEnable()
    {
        _launcherToCopy.OnLaunch += CopyLaunch;
    }

    private void OnDisable()
    {
        _launcherToCopy.OnLaunch += CopyLaunch;
    }

    private void CopyLaunch(BulletLauncher launcher, PatternData pattern)
    {
        float angleStep = pattern.Spread / pattern.Count;
        float aimAngle = pattern.FixedAngle == null ? transform.rotation.eulerAngles.z + pattern.AngleOffset : (float)pattern.FixedAngle + pattern.AngleOffset;
        float centeringOffset = (pattern.Spread / 2) - (angleStep / 2);

        for (int i = 0; i < pattern.Count; i++)
        {
            float currentBulletAngle = angleStep * i;

            aimAngle += UnityEngine.Random.Range(pattern.RandomAngleOffset * -1, pattern.RandomAngleOffset);

            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, aimAngle + currentBulletAngle - centeringOffset));

            Vector2 position = transform.position;
            if (pattern.Position != null) position = (Vector2)pattern.Position;

            Bullet b = Instantiate(pattern.Bullet, position, rotation);

            //b.OnHit += TriggerSpawnedHitEvent;
            b.Initialize(transform.root.gameObject, launcher, pattern.Team);
        }
    }
}
