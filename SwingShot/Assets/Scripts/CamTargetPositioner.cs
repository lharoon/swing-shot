using UnityEngine;

/// <summary>
/// Updates transform of object that camera is following
/// </summary>
public class CamTargetPositioner : MonoBehaviour
{
    public Transform camTargetTransform;

    // Player's distance from camera
    private Vector3 offset;

    // True when player is over stage transition
    private bool isAtEnd = false;

    private float yMax = 0, yMin = -6; // TODO: Update…
    private readonly float yTranslateBy = 6;

    private void Start()
    {
        offset = camTargetTransform.position - transform.position;
    }

    private void FixedUpdate()
    {
        if (!isAtEnd)
        {
            var xPos = transform.position.x + offset.x;
            camTargetTransform.position = new Vector3(xPos, camTargetTransform.position.y);
        }
        else
        {
            var newPos = transform.position + offset;
            // Clamp target y pos
            float yPos = Mathf.Clamp(newPos.y, yMin, yMax);
            newPos = new Vector2(newPos.x, yPos);

            camTargetTransform.position = newPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("goal"))
            isAtEnd = collision.transform.parent.CompareTag("end");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("goal"))
        {
            if (collision.transform.parent.CompareTag("start"))
            {
                yMax -= yTranslateBy; yMin -= yTranslateBy;
            }
        }
    }
}
