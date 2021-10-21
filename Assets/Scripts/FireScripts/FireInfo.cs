using UnityEngine;

public class FireInfo : MonoBehaviour
{
    public bool IsOverPlayer { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            IsOverPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            IsOverPlayer = false;
    }
}
