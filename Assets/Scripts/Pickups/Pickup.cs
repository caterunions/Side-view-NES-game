using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField]
    private float _lifetime;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    private ParticleSystem _collectionParticles;

    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    private float _magnetDistance;

    [SerializeField]
    private float _magnetForce;

    private Coroutine _lifetimeRoutine = null;

    private void OnEnable()
    {
        _lifetimeRoutine = StartCoroutine(LifetimeRoutine());

        rb.velocity = new Vector2(Random.Range(-10f, 10f), 10f);
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position) <= _magnetDistance)
        {
            rb.AddForce((GameManager.Instance.Player.transform.position - transform.position).normalized * _magnetForce * Time.deltaTime);
        }
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(_lifetime * 0.8f);

        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(_lifetime * 0.05f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(_lifetime * 0.05f);
        }

        for (int i = 0; i < 10; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(_lifetime * 0.01f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(_lifetime * 0.01f);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            Instantiate(_collectionParticles, transform.position, Quaternion.identity);
            PickupEffect(playerStats);
            Destroy(gameObject);
        }
    }

    protected abstract void PickupEffect(PlayerStats stats);
}
