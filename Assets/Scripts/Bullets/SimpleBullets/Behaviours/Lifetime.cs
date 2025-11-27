using UnityEngine;

public class Lifetime : BulletBehaviour
{
    [SerializeField]
    private float _lifetime;

    private void OnEnable()
    {
        Destroy(gameObject, _lifetime);
    }
}
