using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.DOFade(0, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);
    }
}
