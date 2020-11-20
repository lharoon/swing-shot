using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PenaltyTextBehaviour : MonoBehaviour
{
    private TextMeshPro text;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
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
