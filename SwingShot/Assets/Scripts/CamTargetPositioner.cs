using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates transform of object that camera is following
/// </summary>
public class CamTargetPositioner : MonoBehaviour
{
    public Transform camTargetTransform;

    // Player's distance from camera
    private Vector3 offset;

    private bool isVertical = false;

    private GameObject lastDoorInStage;
    List<GameObject> lastDoors = new List<GameObject>();
    private Vector2 startOfStage = Vector2.zero, endOfStage;

    private void Start()
    {
        offset = camTargetTransform.position - transform.position;
    }

    private void Update()
    {
        if (lastDoorInStage == null)
        {
            var endObjs = GameObject.FindGameObjectsWithTag("end");
            var lastDoorInStage = ObjUtils.GetNearestObject(endObjs,
                transform.position, lastDoors);
            if (lastDoorInStage != null)
            {
                endOfStage = lastDoorInStage.transform.position;
                lastDoors.Add(lastDoorInStage);
                //print("startOfStage: " + startOfStage); print("endOfStage: " + endOfStage);
            }
        }
    }

    private void FixedUpdate()
    {
        var newPos = transform.position + offset;
        if (!isVertical)
        {
            if (endOfStage.x > startOfStage.x)
                newPos.x = Mathf.Clamp(newPos.x, startOfStage.x, endOfStage.x);
            else
                newPos.x = Mathf.Clamp(newPos.x, endOfStage.x, startOfStage.x);
            newPos.y = startOfStage.y;
        }
        else
        {
            newPos.x = startOfStage.x;
            if (endOfStage.y > startOfStage.y)
                newPos.y = Mathf.Clamp(newPos.y, startOfStage.y, endOfStage.y);
            else
                newPos.y = Mathf.Clamp(newPos.y, endOfStage.y, startOfStage.y);
        }
        camTargetTransform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("goal"))
        {
            var isAtEnd = collision.transform.parent.CompareTag("end");

            if (isAtEnd)
            {
                startOfStage = endOfStage;
                lastDoorInStage = null;
                isVertical = !isVertical; // TODO: Rely on direction property instead
            }
        }
    }
}
