using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public GameObject ringPrefab, bonusCirclePrefab;
    public Shaper2D circleGraphic;
    public AudioSource bgm;

    private float spawnTime = 1.0f;

    private PlayerInfo playerInfo;
    private float recordedHealth;

    private Tween pulseTween, colourTween;
    private bool isRandomisingColour;
    private const float timeToRandomiseColours = 29.0f, bpm = 0.4285714f;

    private void Start()
    {
        StartCoroutine("SpawnRing");

        playerInfo = FindObjectOfType<PlayerInfo>();
        recordedHealth = playerInfo.Health;

        // Pulse graphic to beat (140 bmp)
        pulseTween = circleGraphic.transform.DOScale(1.05f, bpm).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (playerInfo.Health != recordedHealth)
        {
            if (!isRandomisingColour)
            {
                var newColour = Color.Lerp(GameColours.red50, GameColours.blue50, playerInfo.Health);
                colourTween.Kill();
                colourTween = DOTween.To(() => circleGraphic.GetComponent<Shaper2D>().innerColor,
                    x => circleGraphic.GetComponent<Shaper2D>().innerColor = x, newColour, 1);
            }

            DOTween.To(() => circleGraphic.GetComponent<Shaper2D>().innerRadius,
                x => circleGraphic.GetComponent<Shaper2D>().innerRadius = x, playerInfo.Health * 75.0f, 1);

            recordedHealth = playerInfo.Health;

            if (recordedHealth <= 0) // Stop pulsing
                pulseTween.Kill();
        }

        if (bgm.time > timeToRandomiseColours && !isRandomisingColour)
        {
            colourTween.Kill();
            StartCoroutine("RandomiseCircleColour");
            isRandomisingColour = true;
        }
    }

    private IEnumerator SpawnRing()
    {
        while (true)
        {
            Instantiate(ringPrefab, transform);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private IEnumerator RandomiseCircleColour()
    {
        while (recordedHealth > 0) // i.e. while player is alive
        {
            circleGraphic.innerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 0.5f);
            yield return new WaitForSeconds(bpm);
        }

        DOTween.To(() => circleGraphic.GetComponent<Shaper2D>().innerColor,
            x => circleGraphic.GetComponent<Shaper2D>().innerColor = x, GameColours.red50, 1);

        isRandomisingColour = false;
    }

    public void SpawnBonusCircle()
    {
        var bonusCircle = Instantiate(bonusCirclePrefab, transform);
        var shaper2D = bonusCircle.GetComponent<Shaper2D>();
        shaper2D.innerColor = circleGraphic.innerColor;
        shaper2D.outerColor = circleGraphic.outerColor;
    }
}
