using TMPro;
using UnityEngine;

/// <summary>
/// Updates score UI
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public Transform playerTransform;

    private Vector3 startPos;
    private float distanceTravelled;

    private void Start()
    {
        startPos = playerTransform.position;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        distanceTravelled = Vector3.Distance(startPos, playerTransform.position);
        scoreText.text = distanceTravelled.ToString("F1") + " m";
    }
}
