using UnityEngine;
using DG.Tweening;

/// <summary>
/// Fades the attached sprite component to the requested value
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFader : MonoBehaviour
{
    public float endValue, duration;

    private void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.DOFade(endValue, duration);
    }
}
