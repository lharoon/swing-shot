using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BonusTextBehaviour : MonoBehaviour
{
    private TextMeshPro text;
    private ScoreUpdater scoreUpdater;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();

        scoreUpdater = FindObjectOfType<ScoreUpdater>();
        switch (scoreUpdater.Combo)
        {
            case 0:
                text.text = "+2";
                break;
            case 1:
                text.text = "+3";
                break;
            case 2:
                text.text = "+4";
                break;
            case 3:
                text.text = "+5";
                break;
        }

        StartCoroutine(FadeOutTextAfterT(0.25f));
    }

    private void Update()
    {
        if (text.alpha <= 0)
            Destroy(gameObject);
    }

    IEnumerator FadeOutTextAfterT(float t)
    {
        yield return new WaitForSeconds(t);
        GetComponent<TextMeshPro>().DOFade(0, 0.5f);
    }
}
