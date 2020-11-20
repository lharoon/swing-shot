using UnityEngine;

public class DoorDisabler : MonoBehaviour
{
    public GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.CompareTag("end"))
                door.SetActive(false);
        }
    }
}
