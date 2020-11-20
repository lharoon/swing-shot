using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(LightningBolt2D))]
public class LaserColourSwapper : MonoBehaviour, IColourSwapper
{
    public Color newColour = GameColours.black;

    private LightningBolt2D bolt2D;

    private FireInfo fireInfo;
    private Color originalColour;
    private int originalOrder;

    private bool isNewColour;

    private void Start()
    {
        bolt2D = GetComponent<LightningBolt2D>();
        originalColour = bolt2D.lineColor;
        originalOrder = bolt2D.orderInLayer;

        fireInfo = FindObjectOfType<FireInfo>();
    }

    private void Update()
    {
        if (fireInfo == null) return;

        if (fireInfo.IsOverPlayer)
        {
            //if (!isNewColour)
            ChangeColour();
        }
        else if (!fireInfo.IsOverPlayer)
        {
            //if (isNewColour)
            ChangeBackColour();
        }
    }

    public void ChangeColour()
    {
        DOTween.To(() => bolt2D.lineColor, x => bolt2D.lineColor = x, newColour, 0.5f);
        bolt2D.orderInLayer = GameColours.fireOrderInLayer + originalOrder;
        isNewColour = true;
    }

    public void ChangeBackColour()
    {
        DOTween.To(() => bolt2D.lineColor, x => bolt2D.lineColor = x, originalColour, 0.25f);
        bolt2D.orderInLayer = originalOrder;
        isNewColour = false;
    }
}
