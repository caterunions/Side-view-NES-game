using System.Collections.Generic;
using UnityEngine;

using Color = UnityEngine.Color;

public class SplineContainer : MonoBehaviour
{
    [SerializeField]
    private bool _debugDraw = false;

    [SerializeField]
    [Range(1, 25)]
    private int _segmentsPerCurve = 1;

    [SerializeField]
    private List<SplinePoint> _inputPoints = new List<SplinePoint>();

    private List<Vector2> _bezierPoints = new List<Vector2>();

    [SerializeField]
    private List<Vector2> _calculatedPoints = new List<Vector2>();
    public List<Vector2> CalculatedPoints => _calculatedPoints;

    private Vector3 CubicBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        // Eqn: B(t) = (1-t)³ P_0 + 3(1-t)² t*P_1 + 3(1-t)t² P_2 + t³ P_3, t[0,1]
        float amt = 1.0f - t;
        return p0 * Mathf.Pow(amt, 3) + p1 * (3 * amt * amt * t) + p2 * (3 * amt * t * t) + p3 * Mathf.Pow(t, 3);
    }

    private void GenerateSplinePoints(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int segments)
    {
        int curveSegments = Mathf.Max(1, segments);
        for (int j = 0; j < curveSegments; j++)
        {
            float t = j / (float)curveSegments;

            _calculatedPoints.Add(CubicBezierPoint(p0, p1, p2, p3, t));
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying) return;

        _calculatedPoints.Clear();
        _bezierPoints.Clear();

        for (int i = 0; i < _inputPoints.Count; i++)
        {
            Vector2 handlePoint = new Vector2(
                Mathf.Cos(_inputPoints[i].HandleAngle * Mathf.Deg2Rad),
                Mathf.Sin(_inputPoints[i].HandleAngle * Mathf.Deg2Rad));

            handlePoint *= _inputPoints[i].HandleLength;

            if (i > 0) _bezierPoints.Add(handlePoint + _inputPoints[i].Position);
            _bezierPoints.Add(_inputPoints[i].Position);
            if (i < _inputPoints.Count - 1) _bezierPoints.Add((handlePoint * -1) + _inputPoints[i].Position);
        }

        for (int i = 0; i < _inputPoints.Count - 1; i++)
        {
            _bezierPoints.Add(_inputPoints[i].Position);

            GenerateSplinePoints(
                _bezierPoints[i * 3],
                _bezierPoints[(i * 3) + 1],
                _bezierPoints[(i * 3) + 2],
                _bezierPoints[(i * 3) + 3],
                _segmentsPerCurve
                );
        }

        if (_inputPoints.Count > 0)
        {
            _calculatedPoints.Add(_inputPoints[^1].Position);
        }
    }

    private void OnDrawGizmos()
    {
        if (!_debugDraw) return;

        Gizmos.color = Color.lawnGreen;

        Vector3[] arenaPoints = new Vector3[8]
        {
            new Vector3(-17, -17), new Vector3(17, -17),
            new Vector3(17, 17), new Vector3(17, -17),
            new Vector3(17, 17), new Vector3(-17, 17),
            new Vector3(-17, 17), new Vector3(-17, -17),
        };

        Gizmos.DrawLineList(arenaPoints);

        if (_inputPoints.Count == 0 || _bezierPoints.Count == 0 || _calculatedPoints.Count == 0) return;

        foreach (SplinePoint point in _inputPoints)
        {
            Gizmos.color = Color.hotPink;
            Gizmos.DrawSphere(point.Position, 0.25f);
        }

        for (int i = 0; i < _inputPoints.Count; i++)
        {
            Gizmos.color = Color.orange;

            if (i > 0)
            {
                Gizmos.DrawLine(_bezierPoints[i * 3], _bezierPoints[(i * 3) - 1]);
            }
            if (i < _inputPoints.Count - 1)
            {
                Gizmos.DrawLine(_bezierPoints[i * 3], _bezierPoints[(i * 3) + 1]);
            }
        }

        for (int i = 0; i < _calculatedPoints.Count; i++)
        {
            Gizmos.color = Color.limeGreen;
            if (i > 0)
            {
                Gizmos.DrawLine(_calculatedPoints[i - 1], _calculatedPoints[i]);
            }
        }
    }
}

[System.Serializable]
public struct SplinePoint
{
    public SplinePoint(Vector2 pos, float angle, float handleLength = 1)
    {
        _position = pos;
        _handleAngle = angle;
        _handleLength = handleLength;
    }

    [SerializeField]
    private Vector2 _position;
    public Vector2 Position => _position;

    [SerializeField]
    [Range(0f, 360f)]
    private float _handleAngle;
    public float HandleAngle => _handleAngle;

    [SerializeField]
    [Range(1f, 10f)]
    private float _handleLength;
    public float HandleLength => _handleLength;
}