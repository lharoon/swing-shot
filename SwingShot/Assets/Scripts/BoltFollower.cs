using UnityEngine;

/// <summary>
/// Allows LightningBolt2D component to move with a physics object
/// </summary>
[RequireComponent(typeof(LightningBolt2D))]
public class BoltFollower : MonoBehaviour
{
    public Transform fp1, fp2;

    private LightningBolt2D bolt2D;

    private void Start()
    {
        bolt2D = GetComponent<LightningBolt2D>();
    }

    private void Update()
    {
        bolt2D.startPoint = fp1.position;
        bolt2D.endPoint = fp2.position;
        bolt2D.Generate();
    }
}
