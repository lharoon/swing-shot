using UnityEngine;

/// <summary>
/// Destroys player on collision
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    public FlatFX explosionWhite, explosionBlack;
    public Transform trailTransform;

    private FireInfo fire;
    private bool isDestroyed;

    private void Start()
    {
        fire = FindObjectOfType<FireInfo>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("laser") && !isDestroyed)
            KillPlayer();
    }

    private void KillPlayer()
    {
        if (!fire.IsOverPlayer)
            explosionWhite.AddEffect(transform.position, 2);
        else
            explosionBlack.AddEffect(transform.position, 2);
        trailTransform.parent = null;
        Destroy(gameObject);
        isDestroyed = true;
    }
}
