using UnityEngine;
using DG.Tweening;

public class PlayerSpinner : MonoBehaviour
{
    public Transform playerGraphic;
    public TrailRenderer playerTrail;

    private float newScaleY = -1.0f, spinSpeed = 0.25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coin"))
        {
            playerGraphic.DOScaleY(newScaleY, spinSpeed);
            newScaleY *= -1;
        }
    }
}
