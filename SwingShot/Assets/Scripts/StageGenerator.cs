using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    private LevelManager lm;
    private bool hasGeneratedStage;

    private void Start()
    {
        lm = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasGeneratedStage)
        {
            lm.GenerateStage(transform.position);
            hasGeneratedStage = true;
        }
    }
}
