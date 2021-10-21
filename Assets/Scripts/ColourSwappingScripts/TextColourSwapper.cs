using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
[RequireComponent(typeof(Collider2D))]
public class TextColourSwapper : MonoBehaviour, IColourSwapper
{
    public Color newColour = GameColours.black;

    private TextMeshPro text;

    private FireInfo fireInfo;
    private Color originalColour;
    private int originalOrder;

    private bool swapColour, isNewColour;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
        originalColour = text.color;
        originalOrder = text.sortingOrder;

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
        DOTween.To(() => text.color, x => text.color = x, newColour, 0.5f);
        text.sortingOrder = GameColours.fireOrderInLayer + originalOrder;
        isNewColour = true;
    }

    public void ChangeBackColour()
    {
        DOTween.To(() => text.color, x => text.color = x, originalColour, 0.25f);
        text.sortingOrder = originalOrder;
        isNewColour = false;
    }
}
