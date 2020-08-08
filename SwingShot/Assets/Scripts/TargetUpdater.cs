using UnityEngine;

/// <summary>
/// Notifies level manager to update target
/// </summary>
public class TargetUpdater : MonoBehaviour
{
    private LevelManager lm;
    private bool hasEnteredDoor;

    private void Update()
    {
        if (lm == null)
            lm = FindObjectOfType<LevelManager>();

        if (hasEnteredDoor && (Input.GetKeyUp(GameControls.fireKey) || Input.GetMouseButtonUp(0)))
        {
            //print("Updating target");
            lm.UpdateTarget();
            hasEnteredDoor = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("goal"))
        {
            hasEnteredDoor = true;
        }
    }
}
