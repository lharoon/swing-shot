﻿using UnityEngine;

/// <summary>
/// Open & closes door
/// </summary>
public class DoorBehaviour : MonoBehaviour
{
    public float doorSpeed = 25.0f;

    public Transform doorTransform, doorStopTransform;
    public Transform bullseyeOverlayTransform;
    public GameObject pivot;

    public bool IsActivated { get; private set; }

    private Vector3 originalDoorPos;
    private Padlock padlock;

    private void Start()
    {
        if (doorTransform != null)
            originalDoorPos = doorTransform.position;

        // Ensure pivot is at bullseye's position
        pivot.transform.position = transform.position;

        padlock = GetComponent<Padlock>();
    }

    private void Update()
    {
        if (IsActivated && !padlock.isLocked)
        {
            if (doorTransform != null)
                OpenDoor(doorTransform, doorStopTransform);
            ScaleOverlay(Vector2.one);
            pivot.SetActive(true);
        }
        else
        {
            if (doorTransform != null)
                CloseDoor(doorTransform, originalDoorPos);
            ScaleOverlay(Vector2.zero);
            pivot.SetActive(false);
        }
    }

    private void OpenDoor(Transform door, Transform doorStop)
    {
        var step = doorSpeed * Time.deltaTime;

        door.position = Vector2.MoveTowards(door.position,
            doorStop.position, step);
    }

    private void CloseDoor(Transform door, Vector3 originalPos)
    {
        var factor = 2.0f;
        var step = doorSpeed * Time.deltaTime * factor;

        door.position = Vector2.MoveTowards(door.position,
            originalPos, step);
    }

    /// <summary>
    /// Visual effect for opening/closing
    /// </summary>
    /// <param name="targetSize"></param>
    private void ScaleOverlay(Vector2 targetSize)
    {
        var step = 10f * Time.deltaTime;
        bullseyeOverlayTransform.localScale = Vector2.MoveTowards(
            bullseyeOverlayTransform.localScale,
            targetSize, step);
    }

    #region Activation
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("laser"))
            IsActivated = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("laser"))
            IsActivated = false;
    }
    #endregion
}
