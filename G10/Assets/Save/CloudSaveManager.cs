using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CloudSaveManager : MonoSingleton<CloudSaveManager>
{
    public Action<SavedGameRequestStatus> OnSave;
    public Action<SavedGameRequestStatus> OnLoad;

    public SaveState State { get => state; set => state = value; }
    private SaveState state;
    private BinaryFormatter formatter;

    public static CloudSaveManager instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;           
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Initialize the formatter
        formatter = new BinaryFormatter();
        // Create an Empty state
        state = new SaveState();
    }

    public void UpdateState() 
    {
        state = new SaveState();
        state.Coins = StoreManager.instance.PlayerCoins;
        state.Keys = StoreManager.instance.PlayerKeys;
        state.Keyboards = StoreManager.instance.UnlockedKeys;
        state.Backgrounds = StoreManager.instance.UnlockedBGs;
        state._ChlTime = Title_CanvasManager.instance._ChlTime;
        state.AdFree = StoreManager.instance.adfree;

        state.STATSWordsGuessed = Stats_Manager.instance.WordsGuessed;
        state.STATSLettersPlayed = Stats_Manager.instance.LettersPlayed;
        state.STATSLuckyGuesses = Stats_Manager.instance.LuckyGuesses;
        state.STATSChallangesWon = Stats_Manager.instance.ChallangesWon;
        state.STATSGamesPlayed = Stats_Manager.instance.GamesPlayed;
        state.STATSHighlevel = Stats_Manager.instance.Highlevel;
        state.STATSN_HighestScore = Stats_Manager.instance.N_HighestScore;
        state.STATSC_HighScore = Stats_Manager.instance.C_HighScore;
    }

    // SaveState serializer
    private byte[] SerializeState()
    {
        using (MemoryStream ms = new MemoryStream())
        {
            UpdateState(); //,Stats_Manager.instance.WordsGuessed, Stats_Manager.instance.LettersPlayed, Stats_Manager.instance.LuckyGuesses, Stats_Manager.instance.ChallangesWon, Stats_Manager.instance.GamesPlayed, Stats_Manager.instance.Highlevel, Stats_Manager.instance.N_HighestScore, Stats_Manager.instance.C_HighScore);
            formatter.Serialize(ms, state);
            return ms.GetBuffer();
        }
    }
    private SaveState DeserializeState(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        {
            return (SaveState)formatter.Deserialize(ms);
        }
    }

    // Google Cloud
    public void SaveToCloud(Action<PlayServiceError> errorCallback = null)
    {
        PlayService.Instance.OpenCloudSave(OnSaveResponse, errorCallback);
    }
    private void OnSaveResponse(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            byte[] data = SerializeState();
            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
                .WithUpdatedDescription("Last save : " + DateTime.Now.ToString())
                .Build();

            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.CommitUpdate(meta, update, data, SaveCallback);
        }
        else
            OnSave?.Invoke(status);
    }
    private void SaveCallback(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        OnSave?.Invoke(status);
    }

    public void LoadFromCloud(Action<PlayServiceError> errorCallback = null)
    {
        PlayService.Instance.OpenCloudSave(OnLoadResponse, errorCallback);
    }
    private void OnLoadResponse(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.ReadBinaryData(meta, LoadCallback);
        }
        else
            OnLoad?.Invoke(status);
    }
    private void LoadCallback(SavedGameRequestStatus status, byte[] data)
    {
        state = DeserializeState(data);
        OnLoad?.Invoke(status);
    }
    public void PostChallengeScore(int score)
    {
        //PostToLeaderboard(score,GPGSIds.leaderboard_leaderboard,meta, );
        Social.ReportScore(score, GPGSIds.leaderboard_the_challengers, (bool success) =>
        {
            if (success)
                Debug.Log("Score Posted to Leaderboards");
            else
                Debug.Log("Failed to Post Score to Leaderboards");
        });
    }
    public void PostNormalHighestLevel(int Level)
    {
        //PostToLeaderboard(score,GPGSIds.leaderboard_leaderboard,meta, );
        Social.ReportScore(Level, GPGSIds.leaderboard_limit_breakers, (bool success) =>
        {
            if (success)
                Debug.Log("Score Posted to Leaderboards");
            else
                Debug.Log("Failed to Post Score to Leaderboards");
        });
    }
}
