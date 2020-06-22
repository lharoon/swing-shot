using UnityEngine;
using DG.Tweening;

/// <summary>
/// Shows/hides visual indicators for hooking
/// </summary>
public class Indicator : MonoBehaviour
{
    private Shaper2D[] shapes;

    private void Start()
    {
        shapes = GetComponentsInChildren<Shaper2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Turret" && Input.GetKey(KeyCode.Space))
            FadeShapes(0, 0.15f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Turret")
            FadeShapes(1, 0.15f);
    }

    // TODO: Try scaling instead
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
