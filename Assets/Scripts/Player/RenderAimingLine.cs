using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderAimingLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    private Transform _cursor;

    private void OnEnable()
    {
        _cursor = GameManager.Instance.Cursor;
    }

    private void Update()
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, new Vector3(_cursor.position.x, _cursor.position.y, 0f));
    }
}
