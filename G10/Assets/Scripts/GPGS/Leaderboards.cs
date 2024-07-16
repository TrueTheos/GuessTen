using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using TMPro;

public class Leaderboards : MonoBehaviour
{

    public InputField inputScore;
    public TMP_Text logText;

    public void ShowLeaderboardUI()
    {

        Social.ShowLeaderboardUI();
    }

    public void DoLeaderBoardPost(int _score)
    {
        Social.ReportScore(_score, GPGSIds.leaderboard_the_challengers, (bool success) =>
        {
            if(success)
            {
                logText.text = "Score Posted of : " + _score;
            }
            else
            {
                logText.text = "Score Failed to post";
            }
        });
    }

    public void LeaderBoardPostButton()
    {
        Debug.Log(inputScore.text);
        DoLeaderBoardPost(int.Parse(inputScore.text));
    }

}
