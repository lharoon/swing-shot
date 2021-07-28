using UnityEngine;

public class DebrisBehaviour : MonoBehaviour
{
    private void Start()
    {
        float s = Random.Range(0.25f, 0.75f);
        transform.GetChild(0).localScale = new Vector2(s, s);
        transform.GetChild(0).Rotate(0, 0, Random.Range(0, 360));
    }
}
