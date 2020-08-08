using UnityEngine;

/// <summary>
/// Directs turret at nearest target & fires laser when prompted
/// </summary>
public class TurretController : MonoBehaviour
{
    public bool isDebug = false;

    public GameObject laserEndPointPrefab, laser;
    public Transform turretTransform;

    private GameObject laserEndPoint;
    private Transform targetTransform;

    // Laser graphical components
    private LightningBolt2D bolt2D;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        bolt2D = laser.GetComponent<LightningBolt2D>();
    }

    private void Start()
    {
        bolt2D.endPoint = bolt2D.startPoint;
        meshRenderer = laser.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    private void Update()
    {
        if (targetTransform != null)
        {
            //var d = Vector3.Distance(transform.position, targetTransform.position);
            var d = targetTransform.position - transform.position;
            //print("Distance: " + d);
        }

        if (!Input.GetKey(GameControls.fireKey) && !Input.GetMouseButton(0))
        {
            var target = GetClosestTarget();
            if (target != null) targetTransform = target.transform;
            else targetTransform = null;
        }

        if ((Input.GetKeyDown(GameControls.fireKey) || Input.GetMouseButtonDown(0)) && targetTransform != null)
        {
            //bolt2D.FireOnce();
            ShootLaser();
        }
        else if ((Input.GetKeyUp(GameControls.fireKey) || Input.GetMouseButtonUp(0)) && laserEndPoint != null
            || targetTransform == null)
            KillLaser();

        if ((Input.GetKey(GameControls.fireKey) || Input.GetMouseButton(0) || isDebug) && targetTransform != null)
        {
            //&& laserEndPoint != null)
            DrawLaser();
            laserEndPoint.transform.position = targetTransform.position;
        }

        if (targetTransform != null)
        {
            LookAtTarget();

            bolt2D.startPoint = turretTransform.position;
            bolt2D.endPoint = targetTransform.position;
            bolt2D.Generate();

            //line.SetPosition(0, turretTransform.position);
            //line.SetPosition(1, targetTransform.position);
        }

        //if (gm.HasMissedTarget)
        //{
        //    print("Miss");
        //    bolt2D.lineColor = new Color(255, 54, 62);
        //}
    }

    #region Laser
    private void ShootLaser()
    {
        laserEndPoint = Instantiate(laserEndPointPrefab,
            targetTransform.position, Quaternion.identity);
    }

    private void KillLaser()
    {
        // TODO: Fade out
        meshRenderer.enabled = false;
        Destroy(laserEndPoint);
    }

    private void DrawLaser()
    {
        meshRenderer.enabled = true;
    }
    #endregion

    #region Target
    private GameObject GetClosestTarget()
    {
        var targets = GameObject.FindGameObjectsWithTag("target");

        var closestTarget = ObjUtils.GetNearestObject(targets,
            transform.position);

        return closestTarget;
    }

    // https://www.youtube.com/watch?v=Geb_PnF1wOk
    private void LookAtTarget()
    {
        var dir = targetTransform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    #endregion

    private void OnDestroy()
    {
        Destroy(laserEndPoint);
        Destroy(laser);
    }
}
