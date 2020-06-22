using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game object utilities
/// </summary>
public static class ObjUtils
{
    /// <summary>
    /// Finds nearest object among objects (array)
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static GameObject GetNearestObject(GameObject[] objs, Vector3 pos)
    {
        GameObject nearestObj = null;
        var minDistance = Mathf.Infinity;

        foreach (GameObject obj in objs)
        {
            var distance = Vector3.Distance(obj.transform.position, pos);

            if (distance < minDistance)
            {
                nearestObj = obj;
                minDistance = distance;
            }
        }
        return nearestObj;
    }

    /// <summary>
    /// Finds nearest object among objects (list)
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static GameObject GetNearestObject(List<GameObject> objs, Vector3 pos)
    {
        GameObject nearestObj = null;
        var minDistance = Mathf.Infinity;

        foreach (GameObject obj in objs)
        {
            var distance = Vector3.Distance(obj.transform.position, pos);

            if (distance < minDistance)
            {
                nearestObj = obj;
                minDistance = distance;
            }
        }
        return nearestObj;
    }
}
