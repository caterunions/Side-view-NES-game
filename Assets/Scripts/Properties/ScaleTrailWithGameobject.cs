using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTrailWithGameobject : MonoBehaviour
{
    [SerializeField]
    private float _widthMult;

    [SerializeField]
    private TrailRenderer _trailRenderer;

    private void Update()
    {
        _trailRenderer.widthMultiplier = transform.root.localScale.x * _widthMult;
    }
}
