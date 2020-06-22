using UnityEngine;
using DG.Tweening;

/// <summary>
/// Hides trail when player is destroyed
/// </summary>
[RequireComponent(typeof(TrailRenderer))]
public class TrailBehaviour : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private bool isDetached;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (transform.parent == null && !isDetached)
        {
            isDetached = true;

            DOTween.To(() => trailRenderer.startColor,
                x => trailRenderer.startColor = x, Color.clear, 1);
            DOTween.To(() => trailRenderer.endColor,
                x => trailRenderer.endColor = x, Color.clear, 1);
        }

        if (Mathf.Approximately(trailRenderer.endColor.a, 0))
            Destroy(gameObject);
    }
}
