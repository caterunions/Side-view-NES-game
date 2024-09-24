using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyRandomRotation : MonoBehaviour
{
    [SerializeField]
    private float _minRotation;

    [SerializeField] 
    private float _maxRotation;

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(_minRotation, _maxRotation));
    }
}
