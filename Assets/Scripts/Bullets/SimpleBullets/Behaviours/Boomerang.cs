using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : BulletBehaviour
{
    private bool _hasBoomeranged = false;

    private void Update()
    {
        if(!_hasBoomeranged && bullet.DistanceTravelled >= bullet.Range / 2)
        {
            _hasBoomeranged = true;
            transform.RotateAround(transform.position, new Vector3(0, 0, 1), 180f);
        }
        else if(_hasBoomeranged && bullet.DistanceTravelled <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}