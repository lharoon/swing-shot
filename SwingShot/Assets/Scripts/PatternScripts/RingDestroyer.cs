using System.Collections;
using UnityEngine;

public class RingDestroyer : MonoBehaviour
{
    private float timeToDie = 10.0f;

    private void Start()
    {
        StartCoroutine("KillObj");
    }

    private IEnumerator KillObj()
    {
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}
