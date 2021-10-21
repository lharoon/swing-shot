using UnityEngine;

public enum RotationDir { Anticlockwise, Clockwise }

public class FireMovement : MonoBehaviour
{
    public float speed = 10.0f; // Initial speed
    public Rigidbody2D rb2d; // Rigidbody of parent (ie fire)

    private readonly float bottomSpeed = 7.0f; // Actual speed

    private HingeJoint2D anchor; // Pivot
    private bool isHooked; // Whether or not fire is in contact with pivot

    public Direction Direction { get; private set; } // Current direction of fire
    private float currentRotation;

    // Specifies which way fire should rotate next (clockwise or anticlockwise)
    private RotationDir rotationDir;
    private Vector2 newVelocity;

    private void Start()
    {
        // Set initial direction & velocity, & save initial rotation
        Direction = Direction.Right;
        rb2d.velocity = Vector2.right * speed;
        currentRotation = rb2d.rotation;
    }

    private void Update()
    {
        if (isHooked)
        {
            switch (rotationDir)
            {
                case RotationDir.Anticlockwise:
                    if (rb2d.rotation >= currentRotation + 90.0f)
                    {
                        Unhook();
                        SetVelocityAndRotation(currentRotation + 90.0f);
                    }
                    break;
                case RotationDir.Clockwise:
                    if (rb2d.rotation <= currentRotation - 90.0f)
                    {
                        Unhook();
                        SetVelocityAndRotation(currentRotation - 90.0f);
                    }
                    break;
            }
        }
        else
        {
            // Maintain linear velocity
            rb2d.velocity = rb2d.velocity.normalized * speed;
        }

        // Decelerate to target speed (ie bottomSpeed)
        if (speed > bottomSpeed)
            speed -= Time.deltaTime * 1.0f;
    }

    private void SetVelocityAndRotation(float rotation)
    {
        rb2d.velocity = newVelocity;
        rb2d.SetRotation(rotation);
        rb2d.freezeRotation = true; // Stop fire from rotating beyond target rotation
        transform.parent.rotation = Quaternion.Euler(0, 0, rotation);
        currentRotation = rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fire-pivot"))
        {
            // Allow fire to rotate around pivot
            rb2d.freezeRotation = false;
            Hook(collision); // Attach to pivot
            collision.enabled = false; // Disable collider just in case

            // Get new direction that fire should be moving in
            var newDir = collision.GetComponent<FirePivotInfo>().NewDirection;

            // Update new velocity & rotation direction based on current
            // direction & new
            switch (newDir)
            {
                case Direction.Down: // && direction = Direction.Right
                    newVelocity = Vector2.down * speed;
                    rotationDir = RotationDir.Clockwise;
                    break;
                case Direction.Right:
                    newVelocity = Vector2.right * speed;
                    rotationDir = Direction == Direction.Down ?
                        RotationDir.Anticlockwise : RotationDir.Clockwise;
                    break;
                case Direction.Up: // && direction = Direction.Right
                    newVelocity = Vector2.up * speed;
                    rotationDir = RotationDir.Anticlockwise;
                    break;
            }

            Direction = newDir;
        }
    }

    private void Hook(Collider2D collision)
    {
        anchor = collision.GetComponent<HingeJoint2D>();
        anchor.connectedBody = rb2d;
        isHooked = true;
    }

    private void Unhook()
    {
        if (anchor != null)
        {
            anchor.connectedBody = null;
            anchor = null;
        }
        isHooked = false;
    }
}
