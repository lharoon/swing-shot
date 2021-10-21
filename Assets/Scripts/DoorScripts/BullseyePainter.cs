using UnityEngine;

public class BullseyePainter : MonoBehaviour
{
    private Shaper2D shaper2D;

    private Color outerColour = new Color32(130, 45, 49, 255);

    private void Start()
    {
        shaper2D = GetComponent<Shaper2D>();
    }

    private void Update()
    {
        if (CompareTag("target"))
        {
            shaper2D.innerColor = GameColours.red;
            shaper2D.outerColor = outerColour;
        }
        else
        {
            shaper2D.innerColor = GameColours.black;
            shaper2D.outerColor = GameColours.black;
        }
    }
}
