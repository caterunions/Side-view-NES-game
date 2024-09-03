using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField]
    protected float _maxHealth;
    public float MaxHealth => _maxHealth;
}
