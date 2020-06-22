using UnityEngine;

/// <summary>
/// Destroys game object when hit with laser
/// </summary>
public class DebrisKiller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("laser"))
            Destroy(gameObject);
    }
}
