using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BonusCircleBehaviour : MonoBehaviour
{
    public float speed = 100f;
    private Shaper2D shaper2D;
    private float timeToDie = 2f;

    private void Start()
    {
        shaper2D = GetComponent<Shaper2D>();

        //DOTween.To(() => shaper2D.innerColor.a, x => shaper2D.innerColor.a = x, 0, timeToDie);
        shaper2D.innerColor.a = 0;
        DOTween.To(() => shaper2D.outerColor.a, x => shaper2D.outerColor.a = x, 0, timeToDie);
        StartCoroutine("DestroyObj");
    }

    private void Update()
    {
        //shaper2D.innerRadius += speed * Time.deltaTime;
        shaper2D.outerRadius += speed * Time.deltaTime;
    }

    private IEnumerator DestroyObj() // TODO: Move to utility class
    {
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}
