using UnityEngine;
using DG.Tweening;

/// <summary>
/// Shows/hides visual indicators for hooking
/// </summary>
public class Indicator : MonoBehaviour
{
    private Shaper2D[] shapes;
    private bool isFadingOut, isFadingIn;
    private PlayerInfo playerInfo;

    private void Start()
    {
        shapes = GetComponentsInChildren<Shaper2D>();
        playerInfo = GetComponentInParent<PlayerInfo>();
    }

    private void Update()
    {
        if (playerInfo.IsHooked && !isFadingOut)
        {
            isFadingIn = false;
            FadeShapes(0, 0.15f);
            isFadingOut = true;
        }
        else if (!playerInfo.IsHooked && !isFadingIn)
        {
            isFadingOut = false;
            FadeShapes(1, 0.15f);
            isFadingIn = true;
        }
    }

    private void FadeShapes(float alpha, float fadeSpeed)
    {
        foreach (var s in shapes)
        {
            var innerColourO = new Color(s.innerColor.r, s.innerColor.g, s.innerColor.b, alpha);
            var outerColourO = new Color(s.outerColor.r, s.outerColor.g, s.outerColor.b, alpha);

            DOTween.To(() => s.innerColor, x => s.innerColor = x, innerColourO, fadeSpeed);
            DOTween.To(() => s.outerColor, x => s.outerColor = x, outerColourO, fadeSpeed);
        }
    }
}
