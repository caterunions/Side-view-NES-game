using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepTrailOnDestroy : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _trail;

    private void OnDestroy()
    {
        _trail.transform.parent = null;
        _trail.autodestruct = true;
    }
}
