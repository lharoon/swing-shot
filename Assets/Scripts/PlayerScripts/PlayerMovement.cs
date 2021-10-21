using UnityEngine;

/// <summary>
/// Controls linear motion of player
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public bool isDebug = false;

    public float speed = 10f;
    public Vector2 initialDirection = Vector2.right;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (!isDebug)
            rb2d.velocity = initialDirection * speed;
    }

    private void FixedUpdate()
    {
        if (isDebug)
        {
            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");
            var movement = new Vector2(moveH, moveV);

            rb2d.AddForce(movement * speed);
        }
        else
        {
            // Move at linear velocity
            rb2d.velocity = rb2d.velocity.normalized * speed;

            // Look in direction of travel
            var angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
