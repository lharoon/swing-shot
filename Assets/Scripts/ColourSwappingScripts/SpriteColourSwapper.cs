using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class SpriteColourSwapper : MonoBehaviour, IColourSwapper
{
    public Color newColour = GameColours.black;

    private SpriteRenderer sr;

    private FireInfo fireInfo;
    private Color originalColour;
    private int originalOrder;

    private bool swapColour, isNewColour;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColour = sr.color;
        originalOrder = sr.sortingOrder;

        fireInfo = FindObjectOfType<FireInfo>();
    }

    private void Update()
    {
        if (fireInfo == null) return;

        if (fireInfo.IsOverPlayer && swapColour)
        {
            if (!isNewColour) ChangeColour();
        }
        else if (!fireInfo.IsOverPlayer)
        {
            if (isNewColour) ChangeBackColour();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fire"))
            swapColour = true;
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("fire"))
    //        show = false;
    //}

    public void ChangeColour()
    {
        sr.DOColor(newColour, 0.5f);
        sr.sortingOrder = GameColours.fireOrderInLayer + originalOrder;
        isNewColour = true;
    }

    public void ChangeBackColour()
    {
        sr.DOColor(originalColour, 0.25f);
        sr.sortingOrder = originalOrder;
        isNewColour = false;
    }
}
