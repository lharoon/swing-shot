using UnityEngine;

public class DoorTurretLaserInfo : MonoBehaviour
{
    public bool HasPlayerPast { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            HasPlayerPast = true;
    }
}
