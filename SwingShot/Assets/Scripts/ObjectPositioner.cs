using UnityEngine;

/// <summary>
/// Positions associated object under GUI element
/// </summary>
public class ObjectPositioner : MonoBehaviour
{
    public RectTransform target;

    private void Update()
    {
        transform.position = target.position;
    }
}
