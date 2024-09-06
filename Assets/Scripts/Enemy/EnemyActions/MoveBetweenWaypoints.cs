using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenWaypoints : EnemyAction
{
    [System.Serializable]
    private struct EnemyWaypoint
    {
        [SerializeField]
        private Transform _waypoint;
        public Transform Waypoint => _waypoint;

        [SerializeField]
        private float _velocity;
        public float Velocity => _velocity;
    }

    [SerializeField]
    private List<EnemyWaypoint> _waypoints = new List<EnemyWaypoint>();

    [SerializeField]
    private int _repetitions = 1;

    [SerializeField]
    private float _delayBetweenWaypoints;

    protected override IEnumerator ActionInstructions()
    {
        Transform main = EnemyBrain.transform;

        for(int i = 0; i < _repetitions; i++)
        {
            int j = 0;
            foreach(EnemyWaypoint ew in _waypoints)
            {
                while(main.position != ew.Waypoint.position)
                {
                    Vector2 direction = (ew.Waypoint.position - main.position).normalized;
                    EnemyBrain.Rb.velocity = direction * ew.Velocity;

                    float distance = (main.position - ew.Waypoint.position).magnitude;
                    if (distance < 0.1f)
                    {
                        main.position = ew.Waypoint.position;
                        EnemyBrain.Rb.velocity = Vector2.zero;
                        break;
                    }

                    yield return null;
                }

                if (j < _waypoints.Count) yield return new WaitForSeconds(_delayBetweenWaypoints);

                j++;
            }
        }
    }
}
