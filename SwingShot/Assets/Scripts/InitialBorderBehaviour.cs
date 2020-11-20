using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialBorderBehaviour : MonoBehaviour
{
    public List<SpriteFader> sprites;

    private float timeToDie = 5.0f;

    //private void Start()
    //{
    //    StartCoroutine("DestroyInitialBorder");
    //}

    private IEnumerator DestroyInitialBorder()
    {
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }

    public void FadeSprites()
    {
        foreach (var sprite in sprites)
            sprite.enabled = true;

        StartCoroutine("DestroyInitialBorder");
    }
}
