using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Transform> allDoorPrefabs;
    public List<StageManager> stagePrefabs;

    public Transform initialDoorTransform;

    // Selection of doors for use in current stage
    public List<Transform> doorPrefabsForStage { get; private set; }

    public List<Transform> doorsInLevel { get; set; }
    public int StageCount { get; set; } = 0;

    public int GapAtEnd { get; } = 9;

    private int currentTargetIdx = 0;

    private void Start()
    {
        // Initialise doors in level
        doorsInLevel = new List<Transform>();
        doorsInLevel.Add(initialDoorTransform);

        // Add initial set of doors for generation
        doorPrefabsForStage = new List<Transform>();
        int initialSelection = 1;//5;
        for (int i = 0; i < initialSelection; i++)
        {
            doorPrefabsForStage.Add(allDoorPrefabs[i]);
        }

        // Generate initial stage
        GenerateStage(initialDoorTransform.position);
    }

    public void GenerateStage(Vector2 spawnPos)
    {
        var direction = StageCount % 2 == 0 ?
             Direction.Right : Direction.Down;

        if (StageCount != 0)
        {
            if (direction == Direction.Right)
                spawnPos.x += GapAtEnd;
            else if (direction == Direction.Down)
                spawnPos.y -= GapAtEnd;
        }

        int stageIdx = Random.Range(0, stagePrefabs.Count);
        var stage = Instantiate(stagePrefabs[stageIdx], spawnPos,
            Quaternion.identity);
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
