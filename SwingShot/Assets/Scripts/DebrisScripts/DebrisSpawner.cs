using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    public List<Transform> debrisPrefabs;

    private const int minStage = 1, maxDebrisNum = 8;
    private const float initialRadius = 1.25f, increment = 0.125f, maxRadius = 2.5f;

    // TODO: Spawn & deactivate…
    private void Start()
    {
        var segmentInfo = GetComponentInParent<SegmentInfo>();
        if (segmentInfo == null || segmentInfo.StageNum < minStage) return;

        var debrisPrefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Count)];
        //int debrisNum = Random.Range(4, 9);
        int debrisNum = Random.Range(0, segmentInfo.StageNum + 1);
        if (debrisNum > maxDebrisNum) debrisNum = maxDebrisNum;

        //float r = initialRadius + (increment * (segmentInfo.StageNum - minStage));
        float rMin = initialRadius;
        float rMax = initialRadius + (increment * (segmentInfo.StageNum - minStage));
        float r = Random.Range(rMin, rMax);

        if (r > maxRadius) r = maxRadius;
        var pos = new Vector2(0, r);

        for (int i = 0; i < debrisNum; i++)
        {
            var debris = Instantiate(debrisPrefab, transform);
            debris.GetChild(0).localPosition = pos;
            debris.transform.Rotate(0, 0, 360 / debrisNum * i);
        }

        transform.Rotate(0, 0, Random.Range(0, 360));
    }
}
