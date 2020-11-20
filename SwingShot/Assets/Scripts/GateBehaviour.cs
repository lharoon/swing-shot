using UnityEngine;
using DG.Tweening;
using System.Collections;

public class GateBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CompareTag("goal")) return;

        if (collision.CompareTag("Player"))
        {
            //tag = "Untagged";
            var gate = GetClosestGate();

            var player = collision.transform.parent;
            if (player != null)
            {
                collision.enabled = false;
                player.DOMove(gate.transform.position, 0.1f);
                StartCoroutine(ActivateColliderAfterT(collision, 0.1f));
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

    private IEnumerator ActivateColliderAfterT(Collider2D collider, float activationTime)
    {
        yield return new WaitForSeconds(activationTime);
        collider.enabled = true;
    }
}
