using UnityEngine;

public class SegmentActivator : MonoBehaviour
{
    private LevelManager lm;
    private const int n = 3;

    private void Start()
    {
        lm = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("goal"))
        {
            var segment = collision.GetComponentInParent<SegmentInfo>();

            if (segment.Idx - n >= 0)
                lm.Segments[segment.Idx - n].ActivateChildren(false);

            if (segment.Idx + n < lm.Segments.Count)
                lm.Segments[segment.Idx + n].ActivateChildren(true);
        }
    }
}
