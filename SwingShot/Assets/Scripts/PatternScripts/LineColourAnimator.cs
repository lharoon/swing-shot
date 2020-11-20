using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LineColourAnimator : MonoBehaviour
{
    private void Start()
    {
        var targetColour = transform.GetComponentInParent<Shaper2D>().innerColor;
        var endValue = targetColour;
        GetComponent<Image>().DOColor(endValue, 5.0f).SetLoops(2, LoopType.Yoyo);
    }
}
