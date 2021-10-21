using UnityEngine;

public class StageDestroyer : MonoBehaviour
{
    private bool isBeingDestroyed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBeingDestroyed)
        {
            StartCoroutine(GetComponentInParent<StageManager>().DestroyStage());
            isBeingDestroyed = true;
        }
    }
}
