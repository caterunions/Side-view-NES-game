using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBulletsOnDestroy : BulletBehaviour
{
    [SerializeField]
    private Attack _attack;

    private void OnDestroy()
    {
        int curCount = _attack.Count;
        float curSpread = _attack.Spread;
        float curAngleOffset = _attack.AngleOffsetStart;

        bullet.Launcher.Launch(new PatternData(_attack.Bullet, curCount, curSpread, curAngleOffset, _attack.RandomAngleOffset, bullet.Team, bullet.transform.position, _attack.StartAtFixedAngle ? _attack.FixedAngle : null));
    }
}
