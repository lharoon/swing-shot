using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns level elements
/// </summary>
public class LevelManager : MonoBehaviour
{
    public Transform initialDoorTransform;
    public Transform doorPrefabTransform; // TODO: Make list

    public Transform guideNormalPrefab, guideCornerPrefab;
    public Transform stepPrefab;

    private readonly int minNumDoors = 5, maxNumDoors = 15;
    private readonly int difficultyLength = 10; // Num doors before difficulty increases
    private readonly float initialDoorSpacing = 8f;

    //private int doorCount = 0;

    private Vector2 spawnPos = Vector2.zero;

    private List<Transform> doorTransforms = new List<Transform>();
    private int currentTargetIdx = 0;

    private Vector2 newStageTranslation = new Vector2(6, -6);
    private Vector2 stepTranslation = new Vector2(3, -3);

    private void Start()
    {
        doorTransforms.Add(initialDoorTransform);
        //doorCount++;
        GenerateStage(0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stageNum"></param>
    private void GenerateStage(int stageNum)
    {
        // TODO
        // Every 25 doors decrease gap between doors (per stage)
        // Every 5 doors add door to list…
        // Door rotation…
        // Debris…

        var doorSpacing = initialDoorSpacing - ((int)(doorTransforms.Count / difficultyLength * 2));
        doorSpacing = 8; // TEMP

        if (stageNum == 0)
        {
            spawnPos = initialDoorTransform.position;
            spawnPos.x += doorSpacing;

            Instantiate(guideNormalPrefab).position = initialDoorTransform.position;
        }

        int numDoors = GetNumDoors(stageNum);

        for (int i = 0; i < numDoors; i++)
        {
            bool isEnd = i == numDoors - 1;

            var door = SpawnDoor(); // TODO: Consolidate SpawnDoor & SpawnGuide into single function?

            if (stageNum != 0 && i == 0) // isStart
                door.tag = "start";
            else if (isEnd)
                door.tag = "end";

            // Don't instantiate guide for first door in subsequent stages
            if (stageNum == 0 || i != 0)
                SpawnGuide(isEnd);

            if (isEnd)
                spawnPos += newStageTranslation;
            else
                spawnPos.x += doorSpacing;
        }

        if (stageNum < 3) GenerateStage(++stageNum);
    }

    private int GetNumDoors(int stageNum)
    {
        // Randomise number of doors per stage
        int numDoors = UnityEngine.Random.Range(minNumDoors, maxNumDoors);
        //numDoors = 1; // TEMP

        // Ensure num doors is always even so that last door is not inverted
        // (to direct player downwards)
        if (numDoors % 2 != 0)
            numDoors++;

        return numDoors;
    }

    private Transform SpawnDoor()
    {
        var door = Instantiate(doorPrefabTransform, spawnPos, GetDoorRotation());
        // Flip every other door
        if (doorTransforms.Count % 2 != 0) FlipTransform(door);
        door.parent = transform; // TODO: Attach to Stage game
        doorTransforms.Add(door);
        //doorCount++

        return door;
    }

    private void SpawnGuide(bool isEnd)
    {
        if (isEnd)
        {
            //var guide = Instantiate(guideCornerPrefab);
            //guide.position = spawnPos;
            //if (doorTransforms.Count % 2 == 0) FlipTransform(guide);
            Instantiate(guideCornerPrefab).position = spawnPos;
            Instantiate(stepPrefab).position = spawnPos + stepTranslation;
        }
        else
            Instantiate(guideNormalPrefab).position = spawnPos;
    }

    private static void FlipTransform(Transform transform)
    {
        transform.localScale *= new Vector2(1, -1);
    }

    private Quaternion GetDoorRotation()
    {
        // TODO: Rotate at harder stages
        float doorAngle = -45;
        return Quaternion.Euler(new Vector3(0, 0, doorAngle));
    }

    public void UpdateTarget()
    {
        var current = doorTransforms[currentTargetIdx].Find("Fixture/Bullseye");
        var next = doorTransforms[++currentTargetIdx].Find("Fixture/Bullseye");

        current.tag = "Untagged"; next.tag = "target";
    }
}
