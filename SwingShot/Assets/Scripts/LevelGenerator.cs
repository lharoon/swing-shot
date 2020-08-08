using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour // TODO: Rename to StageGenerator...
{
    public List<Transform> allDoorPrefabs;
    public List<StageManager> stagePrefabs;

    // Selection of doors for use in current stage
    public List<Transform> doorPrefabsForStage { get; private set; }

    public int numOfStages; // TEMP

    //private Vector2 spawnPos;

    // …
    public List<Transform> doorsInLevel { get; set; }

    private int currentTargetIdx = 0;

    public int StageCount { get; set; } = 0;

    public Transform initialDoorTransform;

    private void Start()
    {
        doorsInLevel = new List<Transform>();
        doorsInLevel.Add(initialDoorTransform);

        doorPrefabsForStage = new List<Transform>();
        int initialSelection = 5;
        for (int i = 0; i < initialSelection; i++)
        {
            doorPrefabsForStage.Add(allDoorPrefabs[i]);
        }

        GenerateLevel(transform.position, Direction.Right);
    }

    public void GenerateLevel(Vector2 spawnPos, Direction direction)
    {
        int stageIdx = Random.Range(0, stagePrefabs.Count);
        var stage = Instantiate(stagePrefabs[stageIdx], spawnPos, Quaternion.identity);
        stage.direction = direction;
        stage.SpawnDoors();
        StageCount++;
    }

    public void UpdateTarget()
    {
        var current = doorsInLevel[currentTargetIdx].Find("Fixture/Bullseye");
        var next = doorsInLevel[++currentTargetIdx].Find("Fixture/Bullseye");

        current.tag = "Untagged"; next.tag = "target";
    }
}
