using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndGameStats : MonoBehaviour
{

    public static EndGameStats instance;

    [SerializeField] private TMP_Text Score, Guesses, Luckys, TimeElap, Level, ChalWon,coinsEarned;
    [SerializeField] public TMP_Text WinScore, WinGuesses, WinLuckys, WinTimeEl, WinCount, WinCoins;



    private void Awake()
    {
        instance = this;
    }



    public void GameOverStats(bool CHL, int endScore, int endGuesses, int endLevel,int endWinCount, int endLuckys, string endTimeElap, int coins)
    {
        if (CHL)
        {
            Score.text = "SCORE: " + endScore.ToString();
            Guesses.text = "GUESSES: " + endGuesses.ToString();
            ChalWon.text = "Challenges Won: " + endWinCount.ToString();
            Luckys.text = "Lucky Guesses: " + endLuckys.ToString();
            TimeElap.text = "Time Elapsed: " + endTimeElap;
            coinsEarned.text = coins.ToString();
            StoreManager.instance.UpdateCoins(coins);
        }
        else
        {
            Score.text = "SCORE: " + endScore.ToString();
            Guesses.text = "GUESSES: " + endGuesses.ToString();
            Level.text = "Levels Cleared: " + endLevel.ToString();
            Luckys.text = "Lucky Guesses: " + endLuckys.ToString();
            TimeElap.text = "Time Elapsed: " + endTimeElap;
            coinsEarned.text = coins.ToString();
            StoreManager.instance.UpdateCoins(coins);
        }
    }

    public void ChallengeWinStats(int endScore, int endGuesses, int endWinCount, int endLuckys, string endTimeElap, int coins)
    {
        WinScore.text = "SCORE: " + endScore.ToString();
        //Debug.Log("Win SCORE is : " + WinScore.text);
        WinGuesses.text = "GUESSES: " + endGuesses.ToString();
        WinCount.text = "Challenges Won: " + endWinCount.ToString();
        WinLuckys.text = "Lucky Guesses: " + endLuckys.ToString();
        WinTimeEl.text = "Time Elapsed: " + endTimeElap;
        WinCoins.text = coins.ToString();
        StoreManager.instance.UpdateCoins(coins);
        //CloudSaveManager.instance.UpdateState((CloudSaveManager.instance.State.Coins + coins), (CloudSaveManager.instance.State.Keys + 5), CloudSaveManager.instance.State.Keyboards, Title_CanvasManager.instance._ChlTime);
        //CloudSaveTest.instance.Save();
    }


}
