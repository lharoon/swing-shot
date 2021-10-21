using UnityEngine;
using DG.Tweening;

public class PlayerSpinner : MonoBehaviour
{
    public Transform playerGraphic;
    public TrailRenderer playerTrail;

    private float newScaleY = -1.0f, spinSpeed = 0.4f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coin"))
        {
            playerGraphic.DOScaleY(newScaleY, spinSpeed);
            newScaleY *= -1;

            DOTween.To(() => playerTrail.startWidth, x => playerTrail.startWidth = x, 0f, 0.2f).OnComplete(ResetTrailWidth);
        }
    }

    private void ResetTrailWidth()
    {
        DOTween.To(() => playerTrail.startWidth, x => playerTrail.startWidth = x, 0.25f, 0.2f);
    }
}
