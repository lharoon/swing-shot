using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    private LevelManager lm;

    private void Start()
    {
        lm = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("end"))
            lm.GenerateStage(collision.transform.position);
    }
}
