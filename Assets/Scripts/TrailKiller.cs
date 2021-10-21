using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailKiller : MonoBehaviour
{
    private TrailRenderer tr;
    private float timeToDie = 1.0f;

    public void KillTrail()
    {
        transform.parent = null;

        // Fade out trail renderer
        DOTween.To(() => tr.startColor, x => tr.startColor = x, Color.clear, timeToDie);
        DOTween.To(() => tr.endColor, x => tr.endColor = x, Color.clear, timeToDie);

        KillObjectAfterT(timeToDie * 2);
    }

    private IEnumerator KillObjectAfterT(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
