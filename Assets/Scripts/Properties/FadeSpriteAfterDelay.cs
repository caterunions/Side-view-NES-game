using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSpriteAfterDelay : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _delay;

    [SerializeField]
    private float _fadeTime;

    private void OnEnable()
    {
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(_delay);

        while(_spriteRenderer.color.a > 0f)
        {
            Color color = _spriteRenderer.color;
            color.a -= Time.deltaTime / _fadeTime;
            _spriteRenderer.color = color;
            yield return null;
        }
    }
}
