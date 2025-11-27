using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField]
    protected float _maxHealth;
    public float MaxHealth => _maxHealth;
}
