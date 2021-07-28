using UnityEngine;
using DG.Tweening;
using System.Collections;

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

            //playerTrail.startWidth
            DOTween.To(() => playerTrail.startWidth, x => playerTrail.startWidth = x, 0f, 0.2f).OnComplete(Blah);
            //DOTween.To(() => playerTrail.endWidth, x => playerTrail.endWidth = x, 0f, 0.5f).OnComplete(Boo);
        }
    }

    private void Blah()
    {
        DOTween.To(() => playerTrail.startWidth, x => playerTrail.startWidth = x, 0.25f, 0.2f);
    }

    private void Boo()
    {
        DOTween.To(() => playerTrail.endWidth, x => playerTrail.endWidth = x, 0.25f, 0.2f);
    }
}
