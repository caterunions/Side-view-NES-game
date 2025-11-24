using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Homing : BulletBehaviour
{
    [SerializeField]
    private float _maxTurnDegrees;

    [SerializeField]
    private float _homingDistance;

    private void Update()
    {
        List<Transform> _enemies = GameManager.Instance.EnemySpawner.EnemyTransforms;

        if (_enemies.Count == 0) return;

        float lowestDist = Mathf.Infinity;
        Transform closest = _enemies.First();
        foreach (Transform t in _enemies)
        {
            float dist = Vector2.Distance(transform.position, t.position);

            if(dist < lowestDist)
            {
                lowestDist = dist;
                closest = t;
            }
        }

        if (lowestDist > _homingDistance) return;

        Vector3 dir = closest.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * _maxTurnDegrees);
    }
}
