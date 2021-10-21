using UnityEngine;

[RequireComponent(typeof(Shaper2D))]
[RequireComponent(typeof(Collider2D))]
public class Shaper2DOverlayAdder : MonoBehaviour, IOverlayAdder
{
    private Color overlayColour = GameColours.black;
    private Shaper2D shaper2D, overlayShape;

    private bool show;

    private FireInfo fireInfo;

    private void Start()
    {
        shaper2D = GetComponent<Shaper2D>();
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
            if (overlayShape.innerColor.a <= 1)
                ShowOverlay();
        }
        else if (!fireInfo.IsOverPlayer)
        {
            if (overlayShape.innerColor.a >= 0)
                HideOverlay();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fire"))
            show = true;
    }

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
        overlay.transform.parent = transform;
        overlay.name = name + "Overlay";

        // Add shape component
        overlayShape = overlay.AddComponent<Shaper2D>();
        overlayShape.sectorCount = shaper2D.sectorCount;
        overlayShape.arcDegrees = shaper2D.arcDegrees;
        overlayShape.rotation = shaper2D.rotation;
        overlayShape.innerRadius = shaper2D.innerRadius;
        overlayShape.outerRadius = shaper2D.outerRadius;
        overlayShape.starrines = shaper2D.starrines;
        overlayShape.innerColor = overlayColour;
        overlayShape.outerColor = overlayColour;
        overlayShape.orderInLayer = shaper2D.orderInLayer + GameColours.fireOrderInLayer;
        //overlayShape.UpdateMesh();
    }

    public void ShowOverlay()
    {
        overlayShape.innerColor.a += 5f * Time.deltaTime;
        overlayShape.outerColor.a += 5f * Time.deltaTime;
    }

    public void HideOverlay()
    {
        overlayShape.innerColor.a -= 7f * Time.deltaTime;
        overlayShape.outerColor.a -= 7f * Time.deltaTime;
    }
}
