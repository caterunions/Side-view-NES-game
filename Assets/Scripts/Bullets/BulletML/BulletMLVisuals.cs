using UnityEngine;

public class BulletMLVisuals : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    private float _startingAlpha = 0;

    [SerializeField]
    private float _damage = 0;
    public float Damage => _damage;

    public bool HasHit = false;
    public bool HasGrazed = false;

    private void OnEnable()
    {
        _startingAlpha = _spriteRenderer.color.a;
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0f);
    }

    private void Update()
    {
        if (SpriteRenderer.color.a < _startingAlpha)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a + (Time.deltaTime * 3));
        }
        if (SpriteRenderer.color.a > _startingAlpha)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _startingAlpha);
        }
    }
}