using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailBehaviour : MonoBehaviour
{
    public Transform player;

    private TrailRenderer tr;
    private const float timeToDie = 1.0f;
    private bool isBeingKilled;

    private void Start()
    {
        tr = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (player != null)
            transform.position = player.position;
        else if (!isBeingKilled)
        {
            isBeingKilled = true;

            // Fade out trail renderer
            DOTween.To(() => tr.startColor, x => tr.startColor = x, Color.clear, timeToDie);
            DOTween.To(() => tr.endColor, x => tr.endColor = x, Color.clear, timeToDie);

            KillObjectAfterT(timeToDie * 2);
        }
    }

    private IEnumerator KillObjectAfterT(float t)
    {
        //isBeingKilled = true;
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
