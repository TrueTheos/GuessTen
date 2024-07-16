using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

[Flags]
public enum PlayServiceError : byte
{
    None = 0,
    Timeout = 1,
    NotAuthenticated = 2,
    SaveGameNotEnabled = 4,
    CloudSaveNameNotSet = 8,
}

public class PlayService : MonoSingleton<PlayService>
{
    [Header("Save")]
    [SerializeField] private bool enableSaveGame;
    [SerializeField] private string cloudSaveName = "";
    [SerializeField] private DataSource dataSource;
    [SerializeField] private ConflictResolutionStrategy conflictStrategy;

    private void Awake()
    {
        // Persist through scenes
        DontDestroyOnLoad(this.gameObject);

        PlayGamesClientConfiguration.Builder builder = new PlayGamesClientConfiguration.Builder();

        // Enables saving game progress
        if(enableSaveGame)
            builder.EnableSavedGames();

        PlayGamesPlatform.InitializeInstance(builder.Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    // Authentication
    public void SignIn(Action successCallback = null, Action errorCallback = null)
    {
        try
        {
            Social.localUser.Authenticate((bool success) => 
            {
                if (success)
                    successCallback?.Invoke();
            });
        }
        catch(Exception e)
        {
            Debug.Log(e);
            errorCallback?.Invoke();
        }
    }
    public void SignOut()
    {
        if (Social.localUser.authenticated)
            PlayGamesPlatform.Instance.SignOut();
    }

    // Achievements
    public void ShowAchievement()
    {
        Social.ShowAchievementsUI();
    }
    public void PingAchievement(string achievementID, int steps, Action<bool> callback)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(achievementID, steps, callback);
    }

    // Leaderboard
    public void ShowLeaderboard(string leaderboardID)
    {
        Social.ShowLeaderboardUI();
    }
    public void PostToLeaderboard(int score, string leaderboardID, string meta, Action<bool> callback)
    {
        PlayGamesPlatform.Instance.ReportScore(score, leaderboardID, meta, callback);
    }
    public void GetLeaderboardData(string leaderboardID, Action<LeaderboardScoreData> callback)
    {
        PlayGamesPlatform.Instance.LoadScores(
            leaderboardID,
            LeaderboardStart.PlayerCentered,
            10,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            callback);
    }

    // Save Game
    public void OpenCloudSave(Action<SavedGameRequestStatus,ISavedGameMetadata> callback, Action<PlayServiceError> errorCallback = null)
    {
        PlayServiceError error = global::PlayServiceError.None; 
        if (!Social.localUser.authenticated)
            error |= global::PlayServiceError.NotAuthenticated;
        if (PlayGamesClientConfiguration.DefaultConfiguration.EnableSavedGames)
            error |= global::PlayServiceError.SaveGameNotEnabled;
        if (string.IsNullOrWhiteSpace(cloudSaveName))
            error |= global::PlayServiceError.CloudSaveNameNotSet;

        if (error != global::PlayServiceError.None)
            errorCallback?.Invoke(error);

        var platform = (PlayGamesPlatform)Social.Active;
        try { platform.SavedGame.OpenWithAutomaticConflictResolution(cloudSaveName, dataSource, conflictStrategy, callback); } catch { }
    }
}
