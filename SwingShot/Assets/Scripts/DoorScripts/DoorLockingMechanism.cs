using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockingMechanism : MonoBehaviour
{
    public List<Transform> locks;

    public bool IsOpen { get; set; } // Move to DoorInfo?

    private DoorInfo doorInfo;

    private void Start()
    {
        doorInfo = GetComponentInParent<DoorInfo>();
        SetLock();
    }

    public void SetLock() // If doorInfo is updated call this explicitly
    {
        for (int i = 0; i < doorInfo.NumLocks; i++)
        {
            locks[i].gameObject.SetActive(true);

            if (i > doorInfo.ActivationLevel - 1)
                AnimateLocking(i);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("laser")) return;

        if (doorInfo.ActivationLevel > 0)
            AnimateLocking(--doorInfo.ActivationLevel);

        if (doorInfo.ActivationLevel <= 0)
            IsOpen = true;
    }

    private void AnimateLocking(int idx)
    {
        locks[idx].GetChild(1).DOScaleX(0, 0.2f);
        locks[idx].GetChild(2).DOScaleX(0.05f, 0.2f);
        locks[idx].GetChild(3).DOScaleX(-0.05f, 0.2f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("laser")) return;

        if (doorInfo.ActivationLevel <= 0)
        {
            float t = 0.2f;

            foreach (var l in locks)
            {
                // Animate unlocking
                l.GetChild(1).DOScaleX(1f, t);
                l.GetChild(2).DOScaleX(0, t);
                l.GetChild(3).DOScaleX(0, t);
                t += 0.2f; // Stagger animation
            }

            IsOpen = false;
            doorInfo.ActivationLevel = doorInfo.NumLocks;
        }
    }
}
