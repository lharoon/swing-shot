using UnityEngine;
using DG.Tweening;
using System.Collections;

public class GateBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tag = "Untagged";
            var gate = GetClosestGate();

            var playerTransform = collision.transform.parent;
            if (playerTransform != null)
            {
                float t = 0.1f;
                collision.enabled = false;
                playerTransform.DOMove(gate.transform.position, t);
                StartCoroutine(ActivateCollider(collision, t));
            }

            // TODO: Scale down to 0
            //Destroy(gameObject);
            Destroy(gate);
        }
    }

    private GameObject GetClosestGate()
    {
        var gates = GameObject.FindGameObjectsWithTag("gate");

        var closestGate = ObjUtils.GetNearestObject(gates,
            transform.position);

        return closestGate;
    }

    IEnumerator ActivateCollider(Collider2D collider,
        float activationTime)
    {
        yield return new WaitForSeconds(activationTime);
        collider.enabled = true;
    }
}
