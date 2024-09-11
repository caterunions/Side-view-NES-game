using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _text;
    public TextMeshPro Text => _text;

    [SerializeField]
    private float _ascendSpeed;

    [SerializeField]
    private float _lifetime;

    private void OnEnable()
    {
        Destroy(gameObject, _lifetime);
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + _ascendSpeed * Time.deltaTime);
    }
}
