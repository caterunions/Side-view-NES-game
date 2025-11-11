using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SplineContainer : MonoBehaviour
{
    [SerializeField]
    private bool _drawArenaBox = false;

    [SerializeField]
    [Range(1, 25)]
    private int _segmentsPerCurve = 1;

    [SerializeField]
    private List<SplinePoint> _inputPoints = new List<SplinePoint>();

    private List<Vector2> _calculatedPoints = new List<Vector2>();

    public void AddPoint()
    {
        _inputPoints.Add(new SplinePoint(Vector2.zero, 0));
    }

    private Vector2 QuadraticBezierPoint(Vector2 pos1, Vector2 pos2, Vector2 pos3, float t)
    {
        float amt = 1 - t;
        return (pos1 * (amt * amt)) + (2 * (amt * t * pos2)) + (t * t * pos3);
    }

    private Vector3 CubicBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        // Eqn: B(t) = (1-t)³ P_0 + 3(1-t)² t*P_1 + 3(1-t)t² P_2 + t³ P_3, t[0,1]
        float amt = 1.0f - t;
        return p0 * Mathf.Pow(amt, 3) + p1 * (3 * amt * amt * t) + p2 * (3 * amt * t * t) + p3 * Mathf.Pow(t, 3);
    }

    private void GenerateSplinePoints(Vector2 pos1, Vector2 pos2, Vector2 pos3, int segments)
    {
        int curveSegments = Mathf.Max(1, segments);
        Vector2 modifiedPos2 = pos2;
        bool switched = false;
        for (int j = 0; j < curveSegments; j++)
        {
            float t = j / (float)curveSegments;

            if(t > 0.5f && !switched)
            {
                switched = true;
                Vector2 newStart = QuadraticBezierPoint(pos1, pos2, pos3, 0.5f);
                _calculatedPoints.Add(newStart);
                modifiedPos2 = newStart;
            }

            _calculatedPoints.Add(QuadraticBezierPoint(pos1,modifiedPos2,pos3,t));
        }
    }

    private void OnValidate()
    {
        _calculatedPoints.Clear();

        for (int i = 0; i < _inputPoints.Count; i++)
        {
            if (i > 0 && i < _inputPoints.Count - 1)
            {
                GenerateSplinePoints(
                    _inputPoints[i - 1].Position, // p1
                    _inputPoints[i].Position, // p2
                    _inputPoints[i + 1].Position, // p3
                    _segmentsPerCurve // segments
                    );
            }
            else
            {
                _calculatedPoints.Add(_inputPoints[i].Position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_drawArenaBox)
        {
            Gizmos.color = Color.green;

            Vector3[] arenaPoints = new Vector3[8]
            {
                new Vector3(-18, -12), new Vector3(18, -12),
                new Vector3(18, 15), new Vector3(18, -12),
                new Vector3(18, 15), new Vector3(-18, 15),
                new Vector3(-18, 15), new Vector3(-18, -12),
            };

            Gizmos.DrawLineList(arenaPoints);
        }

        if (_inputPoints == null) return;

        foreach(SplinePoint point in _inputPoints)
        {
            Gizmos.color = Color.hotPink;
            Gizmos.DrawSphere(point.Position, 0.5f);
        }

        for(int i = 0; i < _calculatedPoints.Count; i++)
        {
            Gizmos.color = Color.yellow;
            if(i > 0)
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
    private float _handleAngle;
    public float HandleAngle => _handleAngle;

    [SerializeField]
    private float _handleLength;
    public float HandleLength => _handleLength;
}