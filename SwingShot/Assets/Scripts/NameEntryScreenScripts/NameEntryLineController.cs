using DG.Tweening;
using UnityEngine;

public class NameEntryLineController : MonoBehaviour
{
    public Transform targetPos;
    public float targetXPos = 10f;

    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        targetPos.DOMoveX(targetXPos, 1f);
    }

    private void Update()
    {
        lr.SetPosition(1, targetPos.position);
    }
}
