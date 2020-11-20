using TMPro;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Transform bonusTextPrefab, penaltyTextPrefab;

    public int Score { get; private set; } = 0;
    public int Combo { get; private set; }

    private float distanceAcrossLevel = 0, distanceAcrossStage = 0;
    private Vector2 startOfStage;

    private int bonus = 0, penalty = 0;
    private const int coinPts = 2, maxCombo = 3, penaltyPts = 10;
    private int lastCollectedCoin = -2;

    private Transform bonusText, penaltyText;
    private Vector3 bonusTextOffset = new Vector2(0.5f, 0.5f);
    private Vector3 penaltyTextOffset = new Vector2(0.5f, -0.5f);

    private void Start()
    {
        startOfStage = transform.position;
    }

    private void Update()
    {
        distanceAcrossStage = Vector2.Distance(startOfStage, transform.position);

        Score = (int)distanceAcrossStage + (int)distanceAcrossLevel + bonus - penalty;
        scoreText.text = Score.ToString();

        // Tie text to Player position
        if (bonusText != null)
            bonusText.position = transform.position + bonusTextOffset;
        if (penaltyText != null)
            penaltyText.position = transform.position + penaltyTextOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null &&
            collision.transform.parent.CompareTag("end"))
        {
            distanceAcrossLevel += distanceAcrossStage;
            startOfStage = collision.transform.position;
        }

        if (collision.CompareTag("coin"))
        {
            int coinIdx = collision.GetComponent<CoinInfo>().CoinIdx;

            if (coinIdx == lastCollectedCoin + 1)
            {
                if (Combo < maxCombo)
                    Combo++;
            }
            else
            {
                Combo = 0;
            }

            bonus += coinPts + Combo;
            lastCollectedCoin = coinIdx;

            bonusText = Instantiate(bonusTextPrefab, transform.position + bonusTextOffset, Quaternion.identity);
        }

        if (collision.CompareTag("debris"))
        {
            penalty += penaltyPts;

            penaltyText = Instantiate(penaltyTextPrefab, transform.position + penaltyTextOffset, Quaternion.identity);
        }
    }
}
