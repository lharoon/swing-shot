using UnityEngine;
using DG.Tweening;

public class CoinBehaviour : MonoBehaviour
{
    private ScoreUpdater scoreUpdater;

    private Color32 combo0Clr, combo1Clr, combo2Clr, combo3Clr;

    private void Awake()
    {
        scoreUpdater = FindObjectOfType<ScoreUpdater>();

        if (scoreUpdater == null)
            print("Could not find ScoreUpdater!");

        combo0Clr = GameColours.white;
        combo1Clr = new Color32(160, 217, 233, 255);
        combo2Clr = new Color32(81, 189, 225, 255);
        combo3Clr = GameColours.blue;
    }

    private void Update()
    {
        //switch (scoreUpdater.Combo)
        //{
        //    case 0:
        //        GetComponent<SpriteRenderer>().color = combo0Clr;
        //        break;
        //    case 1:
        //        GetComponent<SpriteRenderer>().color = combo1Clr;
        //        break;
        //    case 2:
        //        GetComponent<SpriteRenderer>().color = combo2Clr;
        //        break;
        //    case 3:
        //        GetComponent<SpriteRenderer>().color = combo3Clr;
        //        break;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Destroy(gameObject);
    }
}
