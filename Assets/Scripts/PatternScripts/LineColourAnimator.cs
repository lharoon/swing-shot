using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LineColourAnimator : MonoBehaviour
{
    private void Start()
    {
        var pattern = GetComponentInParent<PatternManager>();
        var targetColour = pattern.circleGraphic.innerColor;
        var endValue = targetColour;
        GetComponent<Image>().DOColor(endValue, 5.0f).SetLoops(2, LoopType.Yoyo);
    }
}
