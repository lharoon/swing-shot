using UnityEngine;

/// <summary>
/// Places ProtoShape2D points over transforms
/// </summary>
public class ShapeFollower : MonoBehaviour
{
    public Transform[] fp = new Transform[4];

    private ProtoShape2D shape2D;

    private void Start()
    {
        shape2D = GetComponent<ProtoShape2D>();
    }

    private void Update()
    {
        for (int i = 0; i < shape2D.points.Count; i++)
        {
            shape2D.points[i].position = fp[i].localPosition;
        }

        shape2D.UpdateMesh();
    }
}
