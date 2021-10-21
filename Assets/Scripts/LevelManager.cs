using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns & manages the level state
/// Stages can either be spawned on Start or on the fly
/// </summary>
public class LevelManager : MonoBehaviour
{
    public List<Transform> allDoorPrefabs;
    public Transform barrier, gate;
    public List<StageManager> stagePrefabs; // Stage configurations to spawn
    public Transform initialDoor; // First door in level

    public List<SegmentInfo> Segments { get; set; } = new List<SegmentInfo>();
    public List<SegmentInfo> EndSegments { get; set; } = new List<SegmentInfo>();

    public Vector2 NextSpawnPos { get; set; } = default;
    public int GapAtEnd { get; } = 9;

    // Counters
    public int StageCount { get; set; } = 0;
    public int CoinCount { get; set; }
    public int CurrentStage { get; private set; } = 0;

    private int currentTargetIdx = 0;
    private Direction nextDir;
    private Transform playerTransform;
    private bool isLevelGenerated;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!isLevelGenerated)
            transform.position = new Vector2(playerTransform.position.x, 0);
    }

    public void GenerateLevel()
    {
        NextSpawnPos = initialDoor.position;

        // TODO: Spawn another 10 when near end…
        for (int i = 0; i < 10; i++)
            GenerateStage(NextSpawnPos);

        isLevelGenerated = true;
    }

    public void GenerateStage(Vector2 spawnPos)
    {
        // Add appropriate spacing between new stage & previous according to new direction
        if (StageCount != 0)
        {
            if (nextDir == Direction.Right)
                spawnPos.x += GapAtEnd;
            else if (nextDir == Direction.Down)
                spawnPos.y -= GapAtEnd;
            else if (nextDir == Direction.Up)
                spawnPos.y += GapAtEnd;
        }

        var direction = nextDir;

        // Randomise direction of subsequent stage
        if (direction == Direction.Right)
            nextDir = Random.Range(0, 2) == 0 ? Direction.Down : Direction.Up;
        else
            nextDir = Direction.Right;

        // Instantiate random stage (if > 0)
        int stageIdx = 0;
        if (StageCount > 0)
            stageIdx = Random.Range(0, stagePrefabs.Count);
        var stage = Instantiate(stagePrefabs[stageIdx], spawnPos, Quaternion.identity);

        stage.CurrentDirection = direction;
        stage.NextDirection = nextDir;

        // Spawn stage elements
        stage.SpawnStageSegments();
        StageCount++;
    }

    public void UpdateTarget()
    {
        var current = Segments[currentTargetIdx].DoorInfo.bullseye;
        var next = Segments[++currentTargetIdx].DoorInfo.bullseye;
        current.tag = "Untagged"; next.tag = "target";
    }

    public void UpdateCurrentStage()
    {
        CurrentStage++;
    }
}
