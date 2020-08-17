using UnityEngine;
using DG.Tweening;

// TODO: Consolidate with other colour swapping scripts
public class SpriteColourSwapper : MonoBehaviour
{
    private SpriteRenderer sr;
    private FireInfo fire;
    private bool show, isBlack;
    private int originalOrderInLayer;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        fire = FindObjectOfType<FireInfo>();
        originalOrderInLayer = sr.sortingOrder;
    }

    private void Update()
    {
        if (show && fire.IsOverPlayer && !isBlack)
        {
            sr.DOColor(GameColours.black, 0.5f);
            sr.sortingOrder = originalOrderInLayer + GameColours.fireOrderInLayer;
            isBlack = true;
        }
        //else if ((!show || !fire.IsOverPlayer) && isBlack)
        else if (!fire.IsOverPlayer && isBlack)
        {
            sr.DOColor(GameColours.white, 0.25f);
            sr.sortingOrder = originalOrderInLayer;
            isBlack = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fire"))
            show = true;
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("fire"))
    //        show = false;
    //}
}
