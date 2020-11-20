using UnityEngine;

public class SegmentInfo : MonoBehaviour
{
    public int Idx { get; set; } = 0;
    public int StageNum { get; set; }
    public DoorInfo DoorInfo { get; set; }
    public bool IsEndSegment { get; set; }

    public void ActivateChildren(bool isActive)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(isActive);
    }
}
