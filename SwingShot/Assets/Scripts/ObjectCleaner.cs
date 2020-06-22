using UnityEngine;

/// <summary>
/// Destroys old object
/// </summary>
public class ObjectCleaner : MonoBehaviour
{
    private Transform playerTansform;
    private readonly float clearance = 40f;

    private void Start()
    {
        playerTansform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (playerTansform == null) return;

        // TODO: Ensure object is to the left of player
        var distanceFromPlayer = (playerTansform.position - transform.position).magnitude;
        if (distanceFromPlayer > clearance)
            Destroy(gameObject);
    }
}
