using System.Collections;
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
    #endregion

    #region Prefabs for segment elements
    public Transform border10Prefab, border8Prefab, border6Prefab;
    public Transform cornerBorder, cornerDoor, cornerBorder2;
    public FirePivotInfo firePivotPrefab;
    public Transform coinPrefab;
    public List<Transform> debrisPrefabs;
    #endregion

    public Direction CurrentDirection { get; set; }
    public Direction NextDirection { get; set; }

    private LevelManager lm;
    private Quaternion minusFortyFive, zero, fortyFive, ninety, minusNinety;
    private Vector2 doorSpawnPos;

    // For spawning debris
    // 30% @ stage 0, 60% @ stage 10
    const int minChance = 30, maxChance = 60, ramp = 3;

    private int targetNumSecureDoors, actualNumSecureDoors = 0, probabilityIdx = 0;
    private int numSegments;

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

    public void SpawnStageSegments()
    {
        numSegments = GetNumOfSegmentsInStage();

        for (int j = 0; j < numSegments; j++)
        {
            var segment = InstantiateSegment("Segment" + j);

            // Skip very first door
            if (lm.StageCount == 0 && j == 0)
            {
                segment.DoorInfo = FindObjectOfType<DoorInfo>();
                segment.DoorInfo.transform.parent = segment.transform;
                SpawnCoin(segment);
                SpawnBorder(border8Prefab, Quaternion.identity, segment);
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

            bool isLast = j == numSegments - 1;

            // Instantiate random door
            //var selectedDoorIdx = lm.StageCount < lm.allDoorPrefabs.Count ? lm.StageCount : lm.allDoorPrefabs.Count - 1;
            var selectedDoorIdx = Random.Range(0, lm.StageCount + 1);
            if (selectedDoorIdx > lm.allDoorPrefabs.Count - 1)
                selectedDoorIdx = lm.allDoorPrefabs.Count - 1;
            //print("selectedDoorIdx: " + selectedDoorIdx);
            segment.DoorInfo = SpawnDoor(lm.allDoorPrefabs[selectedDoorIdx],
                segment, rotation, isLast);

            // Instantiate coin
            if (lm.StageCount != 0 && j == 0)
                SpawnCoin(segment, true);
            SpawnCoin(segment);

            // Spawn debris
            //int r = Random.Range(0, 100);
            //bool spawnDebris = lm.StageCount < 10 ? r < minChance + (ramp * lm.StageCount) : r < maxChance;
            //if (spawnDebris) SpawnDebris(segment);
            //if (Random.Range(0, 3) == 0)
            //    SpawnDebris(segment);

            // Instantiate border with every door except last
            if (!isLast)
            {
                switch (spacing)
                {
                    case 6:
                        SpawnBorder(border6Prefab, Quaternion.identity, segment);
                        break;
                    case 8:
                        SpawnBorder(border8Prefab, Quaternion.identity, segment);
                        break;
                    case 10:
                        SpawnBorder(border10Prefab, Quaternion.identity, segment);
                        break;
                }

                UpdateSpawnPos(spacing);
            }
        }

        var cornerSegment = InstantiateSegment("CornerSegment");
        cornerSegment.IsEndSegment = true;
        lm.EndSegments.Add(cornerSegment);

        // Instantiate corner (border & door)
        if (NextDirection == Direction.Right && CurrentDirection == Direction.Down ||
            NextDirection == Direction.Up)
        {
            SpawnBorder(cornerBorder2, ninety, cornerSegment);
            UpdateSpawnPos(lm.GapAtEnd);
            cornerSegment.DoorInfo = SpawnDoor(cornerDoor, cornerSegment, fortyFive);
            SpawnFirePivot(new Vector2(-4, 4), NextDirection, cornerSegment);
        }
        else if (NextDirection == Direction.Right && CurrentDirection == Direction.Up ||
            NextDirection == Direction.Down)
        {
            SpawnBorder(cornerBorder, Quaternion.identity, cornerSegment);
            UpdateSpawnPos(lm.GapAtEnd);
            cornerSegment.DoorInfo = SpawnDoor(cornerDoor, cornerSegment, minusFortyFive);
            SpawnFirePivot(new Vector2(-4, -4), NextDirection, cornerSegment);
        }

        // Set rotation
        switch (CurrentDirection)
        {
            case Direction.Up:
                transform.localRotation = ninety;
                break;
            case Direction.Down:
                transform.localRotation = minusNinety;
                break;
        }

        lm.NextSpawnPos = cornerSegment.DoorInfo.transform.position;
    }

    private int GetNumOfSegmentsInStage()
    {
        int numSegments = doorAngles.Count * number; // Num segments in stage minus corner segment
        int numSegmentsInLevel = lm.Segments.Count + numSegments + 1; // Total num segments plus corner segment

        // Add door to stage so that polarity of corner door is correct if required
        if (CurrentDirection == Direction.Right && NextDirection == Direction.Up)
        {
            // Force last door to be non-inverted
            if (lm.StageCount != 0) { if (numSegmentsInLevel % 2 != 0) numSegments++; }
            else numSegments++;
        }
        else if (CurrentDirection == Direction.Up && NextDirection == Direction.Right)
        {
            // Force last door to be inverted
            if (numSegmentsInLevel % 2 == 0) numSegments++;
        }
        else if (CurrentDirection == Direction.Right && NextDirection == Direction.Down)
        {
            // Force last door to be inverted
            if (lm.StageCount != 0)
                if (numSegmentsInLevel % 2 == 0) numSegments++;
        }
        else if (CurrentDirection == Direction.Down && NextDirection == Direction.Right)
        {
            // TODO
        }
        else
            print("Unaccounted for scenario!");

        return numSegments;
    }

    private SegmentInfo InstantiateSegment(string segmentName)
    {
        var segment = new GameObject { name = segmentName };
        segment.transform.position = doorSpawnPos;
        segment.transform.parent = transform;
        var segmentInfo = segment.AddComponent<SegmentInfo>();
        segmentInfo.Idx = lm.Segments.Count;
        segmentInfo.StageNum = lm.StageCount;
        lm.Segments.Add(segmentInfo);
        return segmentInfo;
    }

    private void SpawnBorder(Transform borderPrefab, Quaternion quaternion,
        SegmentInfo segment)
    {
        var border = Instantiate(borderPrefab, doorSpawnPos, quaternion);
        border.parent = segment.transform;
        border.gameObject.SetActive(ShouldSegmentBeActive(segment));
    }

    private void SpawnDebris(SegmentInfo segment)
    {
        float halfSpacing = spacing / 2;
        float x = Random.Range(doorSpawnPos.x - halfSpacing, doorSpawnPos.x + halfSpacing);
        float y = Random.Range(doorSpawnPos.y - 5.0f, doorSpawnPos.y + 5.0f);
        // TODO: Randomise rotation & scale
        var debris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Count)],
            new Vector2(x, y),
            Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
        debris.parent = segment.transform;
        debris.gameObject.SetActive(ShouldSegmentBeActive(segment));
    }

    private void SpawnCoin(SegmentInfo segment, bool isFirst = false)
    {
        var coinSpawnPos = doorSpawnPos;
        if (isFirst)
            coinSpawnPos.x -= spacing / 2;
        else
            coinSpawnPos.x += spacing / 2;
        var coin = Instantiate(coinPrefab, coinSpawnPos, Quaternion.identity);
        coin.parent = segment.transform;
        coin.gameObject.SetActive(ShouldSegmentBeActive(segment));
        // lm.CoinCount is incremented in CoinInfo
    }

    private void SpawnFirePivot(Vector2 offset, Direction newDir, SegmentInfo segment)
    {
        var fp = Instantiate(firePivotPrefab, doorSpawnPos + offset, Quaternion.identity);
        fp.NewDirection = newDir;
        fp.transform.parent = segment.transform;
        fp.gameObject.SetActive(ShouldSegmentBeActive(segment));
    }

    private DoorInfo SpawnDoor(Transform doorTransform, SegmentInfo segment,
    Quaternion rot, bool isLast = false)
    {
        var door = Instantiate(doorTransform, doorSpawnPos, rot);
        door.parent = segment.transform;

        // Invert doors alternately
        if (lm.Segments.Count % 2 != 0)
            door.transform.localScale *= new Vector2(1, -1);

        if (isLast)
            door.tag = "penultimate";

        door.gameObject.SetActive(ShouldSegmentBeActive(segment));

        //if (actualNumSecureDoors < targetNumSecureDoors)
        //    DetermineNumLocks(door);

        return door.GetComponent<DoorInfo>();
    }

    private void DetermineNumLocks(Transform door)
    {
        if (Random.Range(probabilityIdx, numSegments) == numSegments - 1)
        {
            door.GetComponent<DoorInfo>().NumLocks = 2;
            door.GetComponent<DoorInfo>().ActivationLevel = Random.Range(1, 3);
            actualNumSecureDoors++;
        }
        else
        {
            probabilityIdx++;
        }
    }

    private void UpdateSpawnPos(float gap)
    {
        var newPos = doorSpawnPos;
        newPos.x += gap;
        doorSpawnPos = newPos;
    }

    private bool ShouldSegmentBeActive(SegmentInfo segment)
    {
        return segment.Idx < 3;
    }

    public IEnumerator DestroyStage()
    {
        yield return new WaitForSeconds(5.0f);
        // Don't destroy stage if game is already over
        if (GameObject.FindGameObjectWithTag("Player") != null)
            Destroy(gameObject);
    }
}
