using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GS_Stats_Handler : MonoBehaviour
{
    public static GS_Stats_Handler instance;

    [SerializeField] public float WordsGuessed, LettersPlayed, LuckyGuesses, ChallangesWon, GamesPlayed, Highlevel, N_HighestScore, C_HighScore;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update

    void Start()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        WordsGuessed = CloudSaveManager.instance.State.STATSWordsGuessed;
        LettersPlayed = CloudSaveManager.instance.State.STATSLettersPlayed;
        LuckyGuesses = CloudSaveManager.instance.State.STATSLuckyGuesses;
        ChallangesWon = CloudSaveManager.instance.State.STATSChallangesWon;
        GamesPlayed = CloudSaveManager.instance.State.STATSGamesPlayed;
        Highlevel = CloudSaveManager.instance.State.STATSHighlevel;
        N_HighestScore = CloudSaveManager.instance.State.STATSN_HighestScore;
        C_HighScore = CloudSaveManager.instance.State.STATSC_HighScore;
    }

    public void AddtoStats(int wordsGuessed, int lettersPlayed, int luckyGuesses, int challangesWon, int gamesPlayed, int highlevel, int n_HighestScore, int c_HighScore)
    {
        LoadStats();
        Stats_Manager.instance.TestFunctionPleaseWork(wordsGuessed, lettersPlayed, luckyGuesses, challangesWon, gamesPlayed, highlevel, n_HighestScore, c_HighScore);
    }
}
