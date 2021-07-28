using UnityEngine;

/// <summary>
/// …
/// </summary>
public class DebrisKiller : MonoBehaviour
{
    private FlatFX debrisExplosion;

    private void Start()
    {
        debrisExplosion = GameObject.Find("Explosion3").GetComponent<FlatFX>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            debrisExplosion.AddEffect(transform.position, 2);
            Destroy(gameObject);
        }
    }
}
