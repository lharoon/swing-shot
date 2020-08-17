using System.Collections;
using UnityEngine;

public class StageDestroyer : MonoBehaviour
{
    private float timeToDie = 10.0f;
    private bool isBeingDestroyed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBeingDestroyed)
        {
            StartCoroutine(DestroyObjAfterT(transform.parent.parent.gameObject, timeToDie));
            isBeingDestroyed = true;
        }
    }

    IEnumerator DestroyObjAfterT(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(obj);
    }
}
