using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO
        if (collision.CompareTag("Player"))
            Destroy(gameObject);
    }
}
