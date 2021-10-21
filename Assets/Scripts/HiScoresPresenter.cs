using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Loads online scores into string
/// </summary>
public class HiScoresPresenter : MonoBehaviour // Rename TopScorersLoader
{
    public TextMeshProUGUI topScorersText;
    public int numScoresToDisplay = 5;

    private dreamloLeaderBoard dl;
    private List<dreamloLeaderBoard.Score> scores = new List<dreamloLeaderBoard.Score>();
    private bool hasDisplayedScores;

    private void Start()
    {
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        dl.GetScores();
    }

    private void Update()
    {
        if (scores.Count == 0)
        {
            scores = dl.ToListHighToLow();
            return;
        }
        else if (!hasDisplayedScores)
        {
            DisplayHiScores();
            hasDisplayedScores = true;
        }
    }

    private void DisplayHiScores()
    {
        string hiScoresStr = "Top scorers\n";

        for (int i = 0; i < numScoresToDisplay; i++)
        {
            if (i > scores.Count - 1)
                break;

            hiScoresStr += (i + 1) + ". " + scores[i].playerName + "\t\t" +
                scores[i].score.ToString() + "\n";
        }

        topScorersText.text = hiScoresStr;
        PlayerPrefs.SetString("topScorers", hiScoresStr);
    }
}
