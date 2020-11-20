using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public GameObject ringPrefab;

    private float spawnTime = 1.0f;

    private PlayerInfo playerInfo;
    private float recordedHealth;

    private void Start()
    {
        StartCoroutine("SpawnRing");

        playerInfo = FindObjectOfType<PlayerInfo>();
        recordedHealth = playerInfo.Health;
    }

    private void Update()
    {
        if (playerInfo.Health != recordedHealth)
        {
            var newColour = Color.Lerp(GameColours.red50, GameColours.blue50, playerInfo.Health);
            DOTween.To(() => GetComponent<Shaper2D>().innerColor,
                x => GetComponent<Shaper2D>().innerColor = x, newColour, 1);

            //DOTween.To(() => GetComponent<Shaper2D>().arcDegrees,
            //    x => GetComponent<Shaper2D>().arcDegrees = x, playerInfo.Health * 360, 1);

            DOTween.To(() => GetComponent<Shaper2D>().innerRadius,
                x => GetComponent<Shaper2D>().innerRadius = x, playerInfo.Health * 75.0f, 1);

            recordedHealth = playerInfo.Health;
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
}
