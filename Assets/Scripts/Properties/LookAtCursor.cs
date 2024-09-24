using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    [SerializeField]
    private float _maxTurnDegrees;

    private void Update()
    {
        Vector3 dir = GameManager.Instance.Cursor.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * _maxTurnDegrees);
    }
}
