﻿using DG.Tweening;
using UnityEngine;

/// <summary>
/// Directs turret at nearest target & fires laser when prompted
/// </summary>
public class TurretController : MonoBehaviour
{
    public bool isDebug = false;

    public GameObject laserEndPointPrefab, laser;
    public Transform turretTransform;
    public AudioSource laserSound;

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
        if (!Input.GetKey(GameControls.fireKey) && !Input.GetMouseButton(0))
        {
            var target = GetClosestTarget();
            if (target != null) targetTransform = target.transform;
            else targetTransform = null;
        }

        if ((Input.GetKeyDown(GameControls.fireKey) || Input.GetMouseButtonDown(0)) &&
            targetTransform != null)
        {
            ShootLaser();
        }
        else if ((Input.GetKeyUp(GameControls.fireKey) || Input.GetMouseButtonUp(0)) &&
            laserEndPoint != null ||
            targetTransform == null)
            KillLaser();

        if ((Input.GetKey(GameControls.fireKey) || Input.GetMouseButton(0) || isDebug) &&
            targetTransform != null)
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
        }
    }

    #region Laser
    private void ShootLaser()
    {
        laserEndPoint = Instantiate(laserEndPointPrefab,
            targetTransform.position, Quaternion.identity);

        laserSound.volume = 1f;
        laserSound.pitch = Random.Range(0.75f, 1.25f);
        laserSound.Play();
    }

    private void KillLaser()
    {
        // TODO: Fade out
        meshRenderer.enabled = false;
        Destroy(laserEndPoint);

        laserSound.Stop();
        //laserSound.DOFade(0f, 0.1f);
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
