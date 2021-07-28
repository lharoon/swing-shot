using UnityEngine;

public class DoorInfo : MonoBehaviour
{
    public Transform bullseye;
    public int NumLocks { get; set; } = 1; // All doors will have minimum of 1 lock
    public int ActivationLevel { get; set; } = 1; // 0 is open
}
