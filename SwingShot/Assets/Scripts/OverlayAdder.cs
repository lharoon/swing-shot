using UnityEngine;

public class OverlayAdder : MonoBehaviour
{
    public int orderInLayer = 200;
    public Color colour, outlineColour;
    public Transform shapeTransform;

    private ProtoShape2D shape2D, overlayShape;
    private bool show;
    private float fadeInSpeed = 5f, fadeOutSpeed = 7f; // TODO: Speed up

    private void Start()
    {
        shape2D = shapeTransform.GetComponent<ProtoShape2D>();
        CreateOverlay();
    }

    private void Update()
    {
        if (show && overlayShape.color1.a <= 1)
        {
            overlayShape.color1.a += fadeInSpeed * Time.deltaTime;
            overlayShape.outlineColor.a += fadeInSpeed * Time.deltaTime;
            overlayShape.UpdateMesh();
        }
        else if (!show && overlayShape.color1.a >= 0)
        {
            overlayShape.color1.a -= fadeOutSpeed * Time.deltaTime;
            overlayShape.outlineColor.a -= fadeOutSpeed * Time.deltaTime;
            overlayShape.UpdateMesh();
        }
    }

    private void CreateOverlay()
    {
        // Instantiate overlay object
        var overlay = new GameObject();
        overlay.transform.position = transform.position;
        overlay.transform.position = transform.position;
        if (transform.parent != null)
        {
            overlay.transform.rotation = transform.parent.rotation;
            overlay.transform.localScale = transform.parent.localScale;
        }
        //overlay.transform.parent = transform;
        overlay.transform.parent = shapeTransform;
        overlay.name = name + "Overlay";

        // Add shape component
        overlayShape = overlay.AddComponent<ProtoShape2D>();
        overlayShape.points = shape2D.points;
        overlayShape.color1 = colour;
        overlayShape.color1.a = 0;
        overlayShape.outlineColor = outlineColour;
        overlayShape.outlineColor.a = 0;
        overlayShape.outlineWidth = 0.1f;
        overlayShape.orderInLayer = orderInLayer;
        overlayShape.UpdateMesh();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fire"))
        {
            //print(name);
            show = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("fire"))
        {
            show = false;
        }
    }
}
