using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyFlipSprite : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        if(Random.Range(0, 2) == 0)
        {
            _spriteRenderer.flipX = true;
        }
    }
}
