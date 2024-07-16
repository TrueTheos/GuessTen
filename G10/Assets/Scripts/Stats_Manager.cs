using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats_Manager : MonoBehaviour
{
    public static Stats_Manager instance;


    [SerializeField] public int WordsGuessed, LettersPlayed, LuckyGuesses, ChallangesWon, GamesPlayed, Highlevel, N_HighestScore, C_HighScore;
    public TMP_Text WordsGuessed_TMP, LettersPlayed_TMP, LuckyGuesses_TMP, ChallangesWon_TMP, GamesPlayed_TMP, Highlevel_TMP, N_HighestScore_TMP, C_HighScore_TMP;


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    public void TestFunctionPleaseWork(int wordsGuessed, int lettersPlayed, int luckyGuesses, int challangesWon, int gamesPlayed, int highlevel, int n_HighestScore, int c_HighScore)
    {
        WordsGuessed += wordsGuessed;
        LettersPlayed += lettersPlayed;
        LuckyGuesses += luckyGuesses;
        ChallangesWon += challangesWon;
        GamesPlayed += gamesPlayed;
        if (Highlevel < highlevel)
            Highlevel = highlevel;
        if (N_HighestScore < n_HighestScore)
            N_HighestScore = n_HighestScore;
        if (C_HighScore < c_HighScore)
            C_HighScore = c_HighScore;
        CloudSaveTest.instance.Save();
    }


    public void SetTitlePlayerStatsTMP(int wordsGuessed, int lettersPlayed, int luckyGuesses, int challangesWon, int gamesPlayed, int highlevel, int n_HighestScore, int c_HighScore)
    {
        WordsGuessed = wordsGuessed;
        LettersPlayed = lettersPlayed;
        LuckyGuesses = luckyGuesses;
        ChallangesWon = challangesWon;
        GamesPlayed = gamesPlayed;
        Highlevel = highlevel;
        N_HighestScore = n_HighestScore;
        C_HighScore = c_HighScore;

        WordsGuessed_TMP.text = WordsGuessed.ToString();
        LettersPlayed_TMP.text = LettersPlayed.ToString();
        LuckyGuesses_TMP.text = LuckyGuesses.ToString();
        ChallangesWon_TMP.text = ChallangesWon.ToString();
        GamesPlayed_TMP.text = GamesPlayed.ToString();
        Highlevel_TMP.text = Highlevel.ToString();
        N_HighestScore_TMP.text = N_HighestScore.ToString();
        C_HighScore_TMP.text = C_HighScore.ToString();
        CloudSaveTest.instance.Save();
    }
    public void StatsSignOut()
    {
        WordsGuessed_TMP.text = "0";
        LettersPlayed_TMP.text = "0";
        LuckyGuesses_TMP.text = "0";
        ChallangesWon_TMP.text = "0";
        GamesPlayed_TMP.text = "0";
        Highlevel_TMP.text = "0";
        N_HighestScore_TMP.text = "0";
        C_HighScore_TMP.text = "0";
    }
}
