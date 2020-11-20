using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopScoresPresenter : MonoBehaviour
{
    public TextMeshProUGUI topScorersLabel;
    public List<TextMeshProUGUI> topScorers, topScores;
    public Image topScorersBg;

    private dreamloLeaderBoard dl;
    private List<dreamloLeaderBoard.Score> scores = new List<dreamloLeaderBoard.Score>();
    private float topScorersBgAlpha;
    private bool isVisible = true, isGettingScores;

    private List<string> topScorersKeys = new List<string> {
        "topScorer1",
        "topScorer2",
        "topScorer3",
        "topScorer4"
    };
    private List<string> topScoresKeys = new List<string>
    {
        "topScore1",
        "topScore2",
        "topScore3",
        "topScore4"
    };


    private void Awake()
    {
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();

        topScorersBgAlpha = topScorersBg.color.a;
    }

    private void Start()
    {
        // Preload top scorers/scores text
        for (int i = 0; i < topScores.Count; i++)
        {
            if (PlayerPrefs.HasKey(topScorersKeys[i]))
                topScorers[i].text = PlayerPrefs.GetString(topScorersKeys[i]);
            if (PlayerPrefs.HasKey(topScoresKeys[i]))
                topScores[i].text = PlayerPrefs.GetInt(topScoresKeys[i]).ToString();

            if (PlayerPrefs.HasKey("blueIdx") && PlayerPrefs.GetInt("blueIdx") == i)
            {
                topScorers[i].color = GameColours.blue;
                topScores[i].color = GameColours.blue;
            }
        }

        // Get & display online scores
        StartCoroutine("SetLatestTopScorers");
    }

    public void HideText()
    {
        FadeText(0);
        topScorersBg.DOFade(0, 0.25f);
        isVisible = false;
    }

    public void ShowText()
    {
        FadeText(1);
        topScorersBg.DOFade(topScorersBgAlpha, 0.25f);
        isVisible = true;
    }

    private void FadeText(float target)
    {
        topScorersLabel.DOFade(target, 0.25f);

        for (int i = 0; i < topScorers.Count; i++)
        {
            topScorers[i].DOFade(target, 0.25f);
            topScores[i].DOFade(target, 0.25f);
        }
    }

    public IEnumerator Refresh(string name, int score)
    {
        bool isUpToDate = false;

        dl.GetScores();

        while (!isUpToDate)
        {
            // Keep checking scores until new entry is registered
            scores = dl.ToListHighToLow();

            foreach (var s in scores)
            {
                if (s.playerName == name &&
                    score <= s.score)
                {
                    isUpToDate = true;
                    break;
                }
            }
            yield return null;
        }

        StartCoroutine("SetLatestTopScorers");
    }

    private IEnumerator GetScores()
    {
        dl.GetScores();
        isGettingScores = true;
        yield return new WaitForSeconds(1);
        isGettingScores = false;
    }

    private IEnumerator SetLatestTopScorers()
    {
        while (scores.Count == 0)
        {
            if (!isGettingScores) StartCoroutine("GetScores");
            scores = dl.ToListHighToLow();

            yield return null;
        }

        for (int i = 0; i < topScorers.Count; i++)
        {
            if (i > scores.Count - 1)
                break;

            UpdateScoreText(i, i, GameColours.white);
            RecordLocalScores(i);
        }

        for (int i = 0; i < scores.Count; i++)
        {
            if (string.Equals(scores[i].playerName,
                PlayerPrefs.GetString("playerName")))
            {
                if (i > topScorers.Count - 1)
                {
                    UpdateScoreText(topScorers.Count - 1, i, GameColours.blue);
                    RecordLocalScores(topScorers.Count - 1);
                    PlayerPrefs.SetInt("blueIdx", topScorers.Count - 1);
                }
                else
                {
                    UpdateScoreTextColour(i, GameColours.blue);
                    PlayerPrefs.SetInt("blueIdx", i);
                }
                break;
            }
        }
    }

    private void RecordLocalScores(int i)
    {
        PlayerPrefs.SetString(topScorersKeys[i], topScorers[i].text);
        PlayerPrefs.SetInt(topScoresKeys[i], scores[i].score);
    }

    private void UpdateScoreText(int textIdx, int scoreIdx, Color textColour)
    {
        string topScorer = (scoreIdx + 1) + ". " + scores[scoreIdx].playerName;
        string topScore = scores[scoreIdx].score.ToString();

        //if (topScorers[textIdx].text != topScorer)
        //    topScorer = "{size}" + topScorer + "{/size}";
        //if (topScores[textIdx].text != topScore)
        //    topScore = "{size}" + topScore + "{/size}";

        //if (PlayerPrefs.GetString(topScorersKeys[textIdx]) != scores[scoreIdx].playerName)
        //    topScorer = "{size}" + topScorer + "{/size}";
        //if (PlayerPrefs.GetInt(topScoresKeys[textIdx]) != scores[scoreIdx].score)
        //    topScore = "{size}" + topScore + "{/size}";

        topScorers[textIdx].text = topScorer;
        topScores[textIdx].text = topScore;
        //topScorers[textIdx].text = (scoreIdx + 1) + ". " + scores[scoreIdx].playerName;
        //topScores[textIdx].text = scores[scoreIdx].score.ToString();

        UpdateScoreTextColour(textIdx, textColour);
    }

    private void UpdateScoreTextColour(int i, Color textColour)
    {
        topScorers[i].color = textColour;
        topScores[i].color = textColour;
        if (!isVisible)
        {
            topScorers[i].alpha = 0;
            topScores[i].alpha = 0;
        }
    }
}
