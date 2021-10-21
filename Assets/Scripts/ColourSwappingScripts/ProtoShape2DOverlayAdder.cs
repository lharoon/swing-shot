using UnityEngine;

[RequireComponent(typeof(ProtoShape2D))]
[RequireComponent(typeof(Collider2D))]
public class ProtoShape2DOverlayAdder : MonoBehaviour, IOverlayAdder
{
    //public Color colour, outlineColour;
    public Transform shapeTransform;

    private Color overlayColour = GameColours.black, overlayOutlineColour = GameColours.white;
    private ProtoShape2D shape2D, overlayShape;

    private bool show;

    private FireInfo fireInfo;

    private void Start()
    {
        shape2D = shapeTransform.GetComponent<ProtoShape2D>();
        CreateOverlay();

        fireInfo = FindObjectOfType<FireInfo>();
    }

    private void Update()
    {
        if (fireInfo == null)
        {
            fireInfo = FindObjectOfType<FireInfo>();
            return;
        }

        if (fireInfo.IsOverPlayer && show)
        {
            if (overlayShape.color1.a <= 1)
                ShowOverlay();
        }
        else if (!fireInfo.IsOverPlayer)
        {
            if (overlayShape.color1.a >= 0)
                HideOverlay();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fire"))
            show = true;
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("fire"))
    //        show = false;
    //}

    public void CreateOverlay()
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
        overlayShape.color1 = overlayColour;
        overlayShape.color1.a = 0;
        overlayShape.outlineColor = overlayOutlineColour;
        overlayShape.outlineColor.a = 0;
        overlayShape.outlineWidth = 0.1f;
        overlayShape.orderInLayer = shape2D.orderInLayer + GameColours.fireOrderInLayer;
        overlayShape.UpdateMesh();
    }

    public void ShowOverlay()
    {
        overlayShape.color1.a += 5f * Time.deltaTime;
        overlayShape.outlineColor.a += 5f * Time.deltaTime;
        overlayShape.UpdateMesh();
    }

    public void HideOverlay()
    {
        overlayShape.color1.a -= 7f * Time.deltaTime;
        overlayShape.outlineColor.a -= 7f * Time.deltaTime;
        overlayShape.UpdateMesh();
    }
}
