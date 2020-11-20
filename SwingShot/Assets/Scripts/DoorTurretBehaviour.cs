using UnityEngine;
using DG.Tweening;

public class DoorTurretBehaviour : MonoBehaviour
{
    public DoorBehaviour doorBehaviour;
    public Transform turretLip, laserTarget;
    public Collider2D laserCollider, normalLine;

    private SpriteRenderer turretGraphic;
    private LightningBolt2D turretLaser;
    private DoorTurretLaserInfo laserInfo;
    private Rigidbody2D playerRigidbody;

    // Flags
    private bool isTurretActivated, isTurretInitialised;
    private bool isShortened, isLengthened = true;

    private float turretLength = 1.0f;

    private void Start()
    {
        turretGraphic = GetComponentInChildren<SpriteRenderer>();
        turretLaser = GetComponentInChildren<LightningBolt2D>();
        laserInfo = GetComponentInChildren<DoorTurretLaserInfo>();
        playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // TODO: Target Player when has passed normal before becoming target

        if (doorBehaviour.CompareTag("target"))
            isTurretActivated = true;

        if (!isTurretActivated || playerRigidbody == null) return;

        if (!isTurretInitialised)
        {
            // Initialise turret
            transform.rotation = GetRotationTowardsPos(GetPosAlongPlayerNormal(10.0f)); // Arbitrary length
            turretGraphic.transform.DOScaleX(turretLength, 0.1f); // Draw turret
            laserCollider.enabled = true; normalLine.enabled = true;
            isTurretInitialised = true;
        }

        if (doorBehaviour.IsActivated)
        {
            if (!isShortened)
            {
                turretGraphic.transform.DOScaleX(turretLength * (2.0f / 3.0f), 0.1f);
                isShortened = true; isLengthened = false;
            }

            turretLaser.GetComponent<MeshRenderer>().enabled = false;

            laserCollider.enabled = false;
        }
        else
        {
            if (!isLengthened)
            {
                turretGraphic.transform.DOScaleX(turretLength, 0.1f);
                isLengthened = true; isShortened = false;
            }

            if (doorBehaviour.CompareTag("target"))
            {
                // Draw laser
                turretLaser.startPoint = turretLip.position;
                turretLaser.endPoint = laserTarget.position;
                turretLaser.GetComponent<MeshRenderer>().enabled = true;
                turretLaser.Generate();

                laserCollider.enabled = true;
            }
        }

        if (laserInfo.HasPlayerPast)
        {
            transform.rotation = GetRotationTowardsPos(playerRigidbody.position);
        }
    }

    private Vector2 GetPosAlongPlayerNormal(float length)
    {
        Direction direction;

        var sm = GetComponentInParent<StageManager>();
        if (sm != null)
            direction = sm.CurrentDirection;
        else
            direction = Direction.Right;

        // Set condition based on current stage direction
        bool check;
        if (direction == Direction.Right)
            check = playerRigidbody.velocity.y >= 0;
        else if (direction == Direction.Down)
            check = playerRigidbody.velocity.x >= 0;
        else //if (sm.CurrentDirection == Direction.Up)
            check = playerRigidbody.velocity.x <= 0;

        if (check)
            return (GetPlayerNormal() * -length) + (Vector2)transform.position;
        else
            return (GetPlayerNormal() * length) + (Vector2)transform.position;
    }

    private Vector2 GetPlayerNormal()
    {
        return new Vector2(playerRigidbody.velocity.y,
            -playerRigidbody.velocity.x).normalized;
    }

    private Quaternion GetRotationTowardsPos(Vector3 pos)
    {
        var dir = pos - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
