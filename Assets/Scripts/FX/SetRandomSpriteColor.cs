using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomSpriteColor : MonoBehaviour
{
    [SerializeField]
    private Color[] _colors;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        _spriteRenderer.color = _colors[Random.Range(0, _colors.Length)];
    }
}
