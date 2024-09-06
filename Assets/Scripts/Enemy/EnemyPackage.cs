using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyPackage : MonoBehaviour
{
    [SerializeField]
    private EnemyBrain _enemyBrain;
    public EnemyBrain EnemyBrain => _enemyBrain;

    [SerializeField]
    private HealthDamageReceiver _healthDamageReceiver;
    public HealthDamageReceiver HealthDamageReceiver => _healthDamageReceiver;

    [SerializeField]
    private SplineContainer _splineContainer;
    public SplineContainer SplineContainer => _splineContainer;

    [SerializeField]
    private SplineAnimate _splineAnimate;
    public SplineAnimate SplineAnimate => _splineAnimate;
}
