using UnityEngine;
using DG.Tweening;

public class ObjectScaler : MonoBehaviour
{
    public float xTarget, yTarget, zTarget;
    public float duration;

    private void Start()
    {
        if (xTarget != transform.position.x)
            transform.DOScaleX(xTarget, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        if (yTarget != transform.position.y)
            transform.DOScaleY(xTarget, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        if (zTarget != transform.position.z)
            transform.DOScaleZ(xTarget, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
