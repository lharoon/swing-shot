using UnityEngine;

/// <summary>
/// Updates transform of object that camera is following
/// </summary>
public class CamTargetPositioner : MonoBehaviour
{
    public Transform camTargetTransform;

    private Vector3 offset; // Player's distance from camera
    private LevelManager lm;
    private int targetStage = 0;
    private bool isVertical = false;
    private Vector2 startOfStage = Vector2.zero, endOfStage;
    private int lastEndSegmentIdx = -1;

    private void Start()
    {
        offset = camTargetTransform.position - transform.position;
        lm = FindObjectOfType<LevelManager>();
    }

    private void FixedUpdate()
    {
        if (endOfStage == Vector2.zero)
        {
            if (lm.EndSegments.Count > 0)
                endOfStage = lm.EndSegments[targetStage++].DoorInfo.transform.position;
            return;
        }

        //var newPos = transform.position + offset;
        Vector3 newPos;
        if (!isVertical)
        {
            if (endOfStage.x > startOfStage.x)
            {
                newPos = transform.position + offset;
                newPos.x = Mathf.Clamp(newPos.x, startOfStage.x, endOfStage.x);
            }
            else // Should never come here
            {
                newPos = transform.position - offset;
                newPos.x = Mathf.Clamp(newPos.x, endOfStage.x, startOfStage.x);
            }
            newPos.y = startOfStage.y;
        }
        else
        {
            if (endOfStage.y > startOfStage.y)
            {
                newPos = transform.position - offset;
                newPos.y = Mathf.Clamp(newPos.y, startOfStage.y, endOfStage.y);
            }
            else
            {
                newPos = transform.position + offset;
                newPos.y = Mathf.Clamp(newPos.y, endOfStage.y, startOfStage.y);
            }
            newPos.x = startOfStage.x;
        }
        camTargetTransform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("goal"))
        {
            var segment = collision.GetComponentInParent<SegmentInfo>();

            if (segment.IsEndSegment && segment.Idx != lastEndSegmentIdx)
            {
                //print("targetStage: " + targetStage);
                startOfStage = endOfStage;
                endOfStage = lm.EndSegments[targetStage++].DoorInfo.transform.position;
                isVertical = !isVertical;
                lastEndSegmentIdx = segment.Idx;
            }
        }
    }
}
