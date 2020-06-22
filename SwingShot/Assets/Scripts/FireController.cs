using UnityEngine;

/// <summary>
/// Moves fire to the right
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class FireController : MonoBehaviour
{
    // Fire is faster to begin with so that it has time to catch up
    // to player on transitioning from title screen
    public float speed = 10f, bottomSpeed = 7f;
    public float decelerationRate = 1f;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.right * speed;
    }

    private void Update()
    {
        if (speed > bottomSpeed)
            speed -= Time.deltaTime * decelerationRate;

        rb2d.velocity = Vector2.right * speed;
    }
}
