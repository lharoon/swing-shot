using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages main game loop
/// </summary>
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI titleText, instructionalText; // Title screen UI
    public GameObject mainCamera;
    public Transform guidePrefab, guideContainer;
    public Transform initialDoorTransform;
    public DoorBehaviour initialDoorBehaviour;

    public GameObject fire;

    private Transform playerTransform;
    private LevelManager lm;

    // Fields for spawning guides on start
    // TODO: Destroy guides in separate script (when distance is > x)
    private float guideSpawnTrigger;
    private const float spawnIncrement = 10f;

    public bool isDebug = false;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        guideSpawnTrigger = playerTransform.position.x + spawnIncrement;
        lm = FindObjectOfType<LevelManager>();

        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        // Debug
        if (Input.GetKeyDown(GameControls.restartKey)
            || Input.GetMouseButtonDown(1))
            SceneManager.LoadScene(0);
    }

    private IEnumerator GameLoop()
    {
        if (!isDebug)
            yield return StartCoroutine(GameStarting());
        yield return StartCoroutine(GamePlaying());
        yield return StartCoroutine(GameEnding());
    }

    private IEnumerator GameStarting()
    {
        print("Game starting");

        while (!Input.GetKeyDown(GameControls.fireKey) && !Input.GetMouseButtonDown(0))
        {
            if (playerTransform.position.x > guideSpawnTrigger)
                SpawnNewGuideOnStart();

            yield return null;
        }
    }

    private void SpawnNewGuideOnStart()
    {
        guideSpawnTrigger += spawnIncrement;
        var guideSpawnPos = new Vector2(guideSpawnTrigger + spawnIncrement, 0);
        var guideInstance = Instantiate(guidePrefab);
        guideInstance.position = guideSpawnPos;
        guideInstance.parent = guideContainer;
    }

    private IEnumerator GamePlaying()
    {
        print("Game playing");

        TransitionToMainCam();

        lm.enabled = true;
        ActivateFire();

        initialDoorTransform.parent = null;

        var roundedXPos = (int)initialDoorTransform.position.x;
        if (roundedXPos % 2 != 0) roundedXPos++;
        var doorPos = new Vector2(roundedXPos, 0);
        initialDoorTransform.position = doorPos;
        initialDoorBehaviour.enabled = true;

        while (playerTransform != null)
            yield return null;
    }

    private void ActivateFire()
    {
        fire.transform.parent = null;
        fire.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        fire.GetComponentInChildren<FireMovement>().enabled = true;
    }

    private void TransitionToMainCam()
    {
        mainCamera.SetActive(true);

        // Hide title screen UI
        titleText.DOFade(0, 0.5f);
        instructionalText.DOFade(0, 0.05f);

        foreach (var guide in GameObject.FindGameObjectsWithTag("guide"))
            guide.GetComponentInChildren<SpriteFader>().enabled = true;
    }

    private IEnumerator GameEnding()
    {
        print("Game ending");

        var fireInfo = fire.GetComponentInChildren<FireInfo>();

        if (!fireInfo.IsOverPlayer)
        {
            // TODO
        }
        else
        {
            // TODO
        }

        throw new NotImplementedException();
    }
}
