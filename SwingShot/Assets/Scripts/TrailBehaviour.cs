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

    private int trailSortingOrder;
    private FireInfo fire;
    private bool isBlack;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailSortingOrder = trailRenderer.sortingOrder;
        fire = FindObjectOfType<FireInfo>();
    }

    private void Update()
    {
        // Switch trail colour when fire collides with player
        if (fire.IsOverPlayer && !isBlack)
        {
            DOTween.To(() => trailRenderer.startColor,
                x => trailRenderer.startColor = x, GameColours.black, 0.5f);
            DOTween.To(() => trailRenderer.endColor,
                x => trailRenderer.endColor = x, GameColours.black, 0.5f);
            trailRenderer.sortingOrder = trailSortingOrder + GameColours.fireOrderInLayer;
            isBlack = true;
        }
        else if (!fire.IsOverPlayer && isBlack)
        {
            DOTween.To(() => trailRenderer.startColor,
                x => trailRenderer.startColor = x, GameColours.white, 0.25f);
            DOTween.To(() => trailRenderer.endColor,
                x => trailRenderer.endColor = x, GameColours.white, 0.25f);
            trailRenderer.sortingOrder = trailSortingOrder;
            isBlack = false;
        }

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
