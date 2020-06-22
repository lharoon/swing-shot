using UnityEngine;

/// <summary>
/// Allows LightningBolt2D component to track transform
/// </summary>
[RequireComponent(typeof(LightningBolt2D))]
public class BoltFollower : MonoBehaviour
{
    public Transform targetTransform;

    private Vector3 offset;
    private LightningBolt2D bolt2D;

    private void Start()
    {
        offset = transform.position - targetTransform.position;
        bolt2D = GetComponent<LightningBolt2D>();
    }

    private void Update()
    {
        var newPos = targetTransform.position + offset;
        bolt2D.startPoint = new Vector2(newPos.x, bolt2D.startPoint.y);
        bolt2D.endPoint = new Vector2(newPos.x, bolt2D.endPoint.y);
        bolt2D.Generate();
   } 
}
