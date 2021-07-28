using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public Transform borderPrefab, borderContainer;
    public GameObject gameScreenCmVCam;
    public TextMeshProUGUI titleText, instructionalText;
    public TextMeshProUGUI menuText1, menuText2;
    public TextMeshProUGUI gameOverText, gameOverInstructionalText;

    public AudioSource bgm, startSound;

    public Color CoinColour { get; private set; }

    private Vector2 borderSpawnPos = new Vector2(15, 0); // Initial door position
    private float borderSpacing = 8.0f;
    //private List<InitialBorderBehaviour> borders = new List<InitialBorderBehaviour>();
    private Queue<InitialBorderBehaviour> borders = new Queue<InitialBorderBehaviour>();
    private const int maxNumBorders = 5;

    private LevelManager lm;
    private PatternManager pattern;

    private int recordedScore;
    private dreamloLeaderBoard dl;
    private TopScoresPresenter topScoresPresenter;

    private bool canRestart;

    private void Start()
    {
        lm = FindObjectOfType<LevelManager>();
        pattern = FindObjectOfType<PatternManager>();

        foreach (Transform child in borderContainer)
        {
            //borders.Add(child.GetComponent<InitialBorderBehaviour>());
            borders.Enqueue(child.GetComponent<InitialBorderBehaviour>());
        }

        // Get online scores
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        //dl.GetScores();
        topScoresPresenter = FindObjectOfType<TopScoresPresenter>();

        StartCoroutine("GameLoop");
    }

    private void Update()
    {
        // For testing
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    print("Deleting all local data!");
        //    PlayerPrefs.DeleteAll();
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //    bgm.time = 25.0f;

        if (!isRandomisingCoinColour)
            StartCoroutine("RandomiseCoinColour");
    }

    private bool isRandomisingCoinColour;

    private IEnumerator RandomiseCoinColour()
    {
        isRandomisingCoinColour = true;

        float t = 1f;
        var randomColour = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        DOTween.To(() => CoinColour, x => CoinColour = x, randomColour, t);
        yield return new WaitForSeconds(t);

        isRandomisingCoinColour = false;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine("GameStart");
        yield return StartCoroutine("GameInProgress");
        yield return StartCoroutine("GameEnd");
    }

    private IEnumerator GameStart()
    {
        print("Game is starting");

        void SpawnBorder()
        {
            var border = Instantiate(borderPrefab, borderContainer);
            border.position = borderSpawnPos;
            //borders.Add(border.GetComponent<InitialBorderBehaviour>());
            borders.Enqueue(border.GetComponent<InitialBorderBehaviour>());
            borderSpawnPos.x += borderSpacing;
        }

        while (
            !Input.GetKeyDown(GameControls.fireKey) &&
            !Input.GetMouseButtonDown(0))
        {
            if (player.position.x > borderSpawnPos.x - borderSpacing)
                SpawnBorder();
            if (borders.Count > maxNumBorders)
            {
                Destroy(borders.Peek().gameObject);
                borders.Dequeue();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                LoadNameEntryScreen();

            yield return null;
        }

        SpawnBorder();
    }

    private IEnumerator GameInProgress()
    {
        print("Game is in progress");

        // Activate main (virtual) camera
        gameScreenCmVCam.SetActive(true);

        // Hide title screen UI & increase opacity of (start) borders
        titleText.DOFade(0, 0.5f);
        //instructionalText.DOFade(0, 0.05f);
        instructionalText.enabled = false;
        //menuText1.DOFade(0, 0.25f); // Move to the left
        //menuText1.transform.DOMoveX()
        menuText1.DOFade(0, 0.25f);
        topScoresPresenter.HideText();
        foreach (var b in borders)
        {
            if (b != null) b.FadeSprites();
        }

        // Generate & position level correctly
        int xPos = (int)lm.transform.position.x;
        if (xPos % 2 != 0) xPos++; // Ensure whole number & even
        lm.transform.position = new Vector2(xPos, 0);
        lm.GenerateLevel();

        // Enable scoring
        var scoreUpdater = player.GetComponentInChildren<ScoreUpdater>();
        scoreUpdater.enabled = true;

        // Enable background effects & start music
        if (pattern != null) pattern.enabled = true;
        bgm.Play();
        //startSound.Play();

        // TODO: Activate fire

        // Continue until player is dead
        while (player != null)
        {
            recordedScore = scoreUpdater.Score; // Local copy of score
            yield return null;
        }
    }

    private IEnumerator GameEnd()
    {
        print("Game is ending");

        // Reveal game over UI
        gameOverText.DOFade(1, 0.3f);
        StartCoroutine("EnableGameOverInstructionalText");
        menuText1.DOFade(1, 0.25f);
        topScoresPresenter.ShowText();

        string name = PlayerPrefs.GetString("playerName");
        dl.AddScore(name, recordedScore);
        StartCoroutine(topScoresPresenter.Refresh(name, recordedScore));

        // Kill music (maybe don't?)
        DOTween.To(() => bgm.volume, x => bgm.volume = x, 0, 0.2f);

        while (true)
        {
            if (canRestart)
            {
                if (
                    Input.GetKeyDown(GameControls.fireKey) ||
                    Input.GetMouseButtonDown(0))
                {
                    DOTween.KillAll();
                    SceneManager.LoadScene(1);
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    LoadNameEntryScreen();
                }
            }
            yield return null;
        }
    }

    private IEnumerator EnableGameOverInstructionalText()
    {
        yield return new WaitForSeconds(1);
        gameOverInstructionalText.gameObject.SetActive(true);
        gameOverInstructionalText.DOFade(1, 1).SetEase(Ease.Linear);
        canRestart = true; // Only allow player to restart once text is visible
        StartCoroutine("FadeOutAndInGameOverInstructionalText");
    }

    private IEnumerator FadeOutAndInGameOverInstructionalText()
    {
        yield return new WaitForSeconds(1);
        gameOverInstructionalText.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuad);
    }

    private void LoadNameEntryScreen()
    {
        DOTween.KillAll();
        PlayerPrefs.DeleteKey("playerName");
        SceneManager.LoadScene(0);
    }
}
