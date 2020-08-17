using System.Collections.Generic;
using UnityEngine;

public enum DoorAngle { MinusFortyFive, Zero, FortyFive }
public enum Direction { Right, Up, Down }

public class StageManager : MonoBehaviour
{
    #region Variables for configuring stage
    public List<DoorAngle> doorAngles;
    public int spacing;
    public int number;
    public Direction direction;
    public Direction nextDirection;
    #endregion

    public Transform border10Prefab, border8Prefab, border6Prefab;
    public Transform cornerBorder, cornerDoor, cornerBorder2;
    public FirePivotInfo firePivotPrefab;

    private LevelManager lm;
    private Quaternion minusFortyFive, zero, fortyFive, ninety, minusNinety;

    private Vector2 doorSpawnPos;

    private void Awake()
    {
        minusFortyFive = Quaternion.Euler(new Vector3(0, 0, -45f));
        zero = Quaternion.Euler(Vector3.zero);
        fortyFive = Quaternion.Euler(new Vector3(0, 0, 45f));
        ninety = Quaternion.Euler(new Vector3(0, 0, 90f));
        minusNinety = Quaternion.Euler(new Vector3(0, 0, -90f));

        lm = FindObjectOfType<LevelManager>();
        transform.parent = lm.transform;
        doorSpawnPos = transform.position;
    }

    public void SpawnDoors()
    {
        int numDoors = doorAngles.Count * number; // Num doors in stage minus corner door
        int numDoorsInLevel = lm.doorsInLevel.Count + numDoors + 1; // Total num doors plus corner door

        // Add door to stage so that polarity of corner door is correct if required
        if (direction == Direction.Right && nextDirection == Direction.Up)
        {
            // Force last door to be non-inverted
            if (lm.StageCount != 0) { if (numDoorsInLevel % 2 != 0) numDoors++; }
            else numDoors++;
        }
        else if (direction == Direction.Up && nextDirection == Direction.Right)
        {
            // Force last door to be inverted
            if (numDoorsInLevel % 2 == 0) numDoors++;
        }
        else if (direction == Direction.Right && nextDirection == Direction.Down)
        {
            // Force last door to be inverted
            if (lm.StageCount != 0)
                if (numDoorsInLevel % 2 == 0) numDoors++;
        }
        else
            print("Unaccounted for scenario!");


        for (int j = 0; j < numDoors; j++)
        {
            // Skip very first door
            if (lm.StageCount == 0 && j == 0)
            {
                UpdateSpawnPos(spacing);
                continue;
            }

            // Get door rotation
            Quaternion rotation;
            switch (doorAngles[j % doorAngles.Count])
            {
                case DoorAngle.MinusFortyFive:
                    rotation = minusFortyFive;
                    break;
                case DoorAngle.Zero:
                    rotation = zero;
                    break;
                default:
                    //case DoorAngles.FortyFive:
                    rotation = fortyFive;
                    break;
            }

            bool isLast = j == numDoors - 1;

            // Instantiate random door
            var selectedDoorIdx = Random.Range(0, lm.doorPrefabsForStage.Count);
            SpawnDoor(lm.doorPrefabsForStage[selectedDoorIdx], rotation, isLast);

            // Instantiate border with every door except last
            if (!isLast)
            {
                switch (spacing)
                {
                    case 6:
                        Instantiate(border6Prefab, doorSpawnPos,
                            Quaternion.identity).parent = transform;
                        break;
                    case 8:
                        Instantiate(border8Prefab, doorSpawnPos,
                            Quaternion.identity).parent = transform;
                        break;
                    case 10:
                        Instantiate(border10Prefab, doorSpawnPos,
                            Quaternion.identity).parent = transform;
                        break;
                }

                UpdateSpawnPos(spacing);
            }
        }

        // Instantiate corner (border & door)
        if (nextDirection == Direction.Right && direction == Direction.Down ||
            nextDirection == Direction.Up)
        {
            Instantiate(cornerBorder2, doorSpawnPos, ninety).parent = transform;
            UpdateSpawnPos(lm.GapAtEnd);
            SpawnDoor(cornerDoor, fortyFive);
            SpawnFirePivot(new Vector2(-4, 4), nextDirection);
        }
        else if (nextDirection == Direction.Right && direction == Direction.Up ||
            nextDirection == Direction.Down)
        {
            Instantiate(cornerBorder, doorSpawnPos, Quaternion.identity).parent = transform;
            UpdateSpawnPos(lm.GapAtEnd);
            SpawnDoor(cornerDoor, minusFortyFive);
            SpawnFirePivot(new Vector2(-4, -4), nextDirection);
        }

        // Set rotation
        switch (direction)
        {
            case Direction.Up:
                transform.localRotation = ninety;
                break;
            case Direction.Down:
                transform.localRotation = minusNinety;
                break;
        }
    }

    private void SpawnFirePivot(Vector2 offset, Direction newDir)
    {
        var fp = Instantiate(firePivotPrefab, doorSpawnPos + offset, Quaternion.identity);
        fp.NewDirection = newDir;
        fp.transform.parent = transform;
    }

    Transform SpawnDoor(Transform doorTransform, Quaternion rot, bool isLast = false)
    {
        var door = Instantiate(doorTransform, doorSpawnPos, rot);
        door.parent = transform;
        lm.doorsInLevel.Add(door);

        // Invert doors alternately
        if (lm.doorsInLevel.Count % 2 != 0)
            door.transform.localScale *= new Vector2(1, -1);

        if (isLast)
            door.tag = "penultimate";

        return door;
    }

    private void UpdateSpawnPos(int gap)
    {
        var newPos = doorSpawnPos;
        newPos.x += gap;
        doorSpawnPos = newPos;
    }
}
