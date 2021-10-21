using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float Health { get; set; } = 1.0f;
    public bool IsHooked { get; set; }

    private void Update()
    {
        if (Health < 1.0f)
        {
            Health += Time.deltaTime / 10.0f;
        }

        //print("Health: " + Health);
    }
}
