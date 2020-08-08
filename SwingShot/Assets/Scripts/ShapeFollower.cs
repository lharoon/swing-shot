using UnityEngine;

/// <summary>
/// Positions points of fire's shape over fp transforms
/// Updates fire's height with screen width
/// </summary>
public class ShapeFollower : MonoBehaviour
{
    public Transform[] fp = new Transform[4];

    private ProtoShape2D shape2D;
    private float camSize;

    private void Start()
    {
        shape2D = GetComponent<ProtoShape2D>();
        camSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        if (Camera.main.orthographicSize != camSize)
        {
            // Update y position of fire points
            float twiceScreenWidth = Camera.main.orthographicSize *
            Screen.width / Screen.height * 2;

            foreach (var p in fp)
            {
                if (p.position.y > 0)
                    p.position = new Vector2(p.position.x, twiceScreenWidth);
                else
                    p.position = new Vector2(p.position.x, -twiceScreenWidth);
            }

            camSize = Camera.main.orthographicSize;
        }

        // Update shape2D
        for (int i = 0; i < shape2D.points.Count; i++)
        {
            shape2D.points[i].position = fp[i].localPosition;
        }

        shape2D.UpdateMesh();
    }
}
