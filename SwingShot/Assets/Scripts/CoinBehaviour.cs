using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    //public List<AudioSource> coinSfx;

    private ScoreUpdater scoreUpdater;
    private GameManager gm;
    private SpriteRenderer sr;
    private PatternManager pm;
    private FlatFX coinVfx;
    //private Transform coinSfx;

    private void Awake()
    {
        scoreUpdater = FindObjectOfType<ScoreUpdater>();

        if (scoreUpdater == null)
            print("Could not find ScoreUpdater!");

        gm = FindObjectOfType<GameManager>();
        sr = GetComponent<SpriteRenderer>();
        pm = FindObjectOfType<PatternManager>();
        coinVfx = GameObject.Find("CoinVFX").GetComponent<FlatFX>();
        //foreach (Transform t in GameObject.Find("CoinSFX").transform)
        //    coinSfx.Add(t.GetComponent<AudioSource>());
    }

    private void Update()
    {
        sr.color = gm.CoinColour;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            pm.SpawnBonusCircle();
            coinVfx.settings[1].start.innerColor = sr.color;
            coinVfx.settings[1].start.outerColor = sr.color;
            coinVfx.AddEffect(transform.position, 1);
            //coinSfx[scoreUpdater.Combo].Play();
        }
    }
}
