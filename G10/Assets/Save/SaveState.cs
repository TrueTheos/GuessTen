using System;
using UnityEngine;

[System.Serializable]
public class SaveState
{
    public int Coins { set; get; }
    public int Keys { set; get; }
    public string Keyboards { set; get; }
    public string Backgrounds { set; get; }
    public DateTime _ChlTime { set; get; }
    public bool AdFree;
    public int STATSWordsGuessed { set; get; }
    public int STATSLettersPlayed { set; get; }
    public int STATSLuckyGuesses { set; get; }
    public int STATSChallangesWon { set; get; }
    public int STATSGamesPlayed { set; get; }
    public int STATSHighlevel { set; get; }
    public int STATSN_HighestScore { set; get; }
    public int STATSC_HighScore { set; get; }
}