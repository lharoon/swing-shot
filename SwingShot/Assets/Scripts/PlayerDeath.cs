using UnityEngine;

/// <summary>
/// Destroys player on collision
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    public FlatFX explosion;
    public Transform trailTransform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("laser"))
            KillPlayer();
    }

    private void KillPlayer()
    {
        explosion.AddEffect(transform.position, 2);
        trailTransform.parent = null;
        Destroy(gameObject);
    }
}
