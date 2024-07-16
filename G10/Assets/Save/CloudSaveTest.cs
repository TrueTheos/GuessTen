using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloudSaveTest : MonoBehaviour
{
    public StoreManager StoreManager_Link;
    public Title_CanvasManager Title_CanvasLink;
    public TMP_Text AccountNameText;

    public static CloudSaveTest instance;

    private bool signedout = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            CloudSaveManager.Instance.OnSave += AfterSave;
            CloudSaveManager.Instance.OnLoad += AfterLoad;
        }
    }

    public void AfterSave(SavedGameRequestStatus status)
    {
        switch (status)
        {
            case SavedGameRequestStatus.Success:
                //debugText.text += "SAVED";
                break;
            default:
                //debugText.text = status.ToString();
                //debugText.text += "WTF";
                break;
        }
    }
    public void AfterLoad(SavedGameRequestStatus status)
    {
        //debugText.text += "AFTER LOAD";
        switch (status)
        {
            case SavedGameRequestStatus.Success:
                //debugText.text += "LOADED";
                MenuScene.instance.loadBlocker.SetActive(false);
              
                try 
                {
                    StoreManager.instance.LoadPlayerCoinsKeys(); 
                } 
                catch {  }

                try
                {
                    StoreManager.instance.LoadPlayerUnlockedKeys(CloudSaveManager.Instance.State.Keyboards);
                }
                catch { }

                try
                {
                    StoreManager.instance.LoadPlayerUnlockedBGs(CloudSaveManager.Instance.State.Backgrounds);
                }
                catch { }

                try
                {
                    Title_CanvasManager.instance.SetChlTime(CloudSaveManager.Instance.State._ChlTime);
                    //Title_CanvasLink._ChlTime = CloudSaveManager.Instance.State._ChlTime;
                }
                catch { }

                try
                {
                    
                    Stats_Manager.instance.SetTitlePlayerStatsTMP(
                        CloudSaveManager.Instance.State.STATSWordsGuessed,
                        CloudSaveManager.Instance.State.STATSLettersPlayed,
                        CloudSaveManager.Instance.State.STATSLuckyGuesses,
                        CloudSaveManager.Instance.State.STATSChallangesWon,
                        CloudSaveManager.Instance.State.STATSGamesPlayed,
                        CloudSaveManager.Instance.State.STATSHighlevel,
                        CloudSaveManager.Instance.State.STATSN_HighestScore,
                        CloudSaveManager.Instance.State.STATSC_HighScore
                    );
                }
                catch { }
                try
                {
                    StoreManager.instance.LoadAdBool(CloudSaveManager.Instance.State.AdFree);
                }
                catch {  }
                

                break;
            default:
                MenuScene.instance.loadBlocker.SetActive(false);
                break;
        }
    }
    private void OnPlayError(PlayServiceError obj)
    {

    }

    public void Save()
    {
        CloudSaveManager.instance.UpdateState();
        CloudSaveManager.Instance.SaveToCloud(OnPlayError);
       // debugText.text += "Saving to cloud...";
    }
    public void Load()
    {
        CloudSaveManager.Instance.LoadFromCloud(OnPlayError);
    }
    public void Login()
    {
        PlayService.Instance.SignIn(OnLoginSuccess,OnLoginFail);
        Title_CanvasLink.isSignedIn();

    }
    public void ShowAchievements()
    {
        PlayService.Instance.ShowAchievement();

    }
    public void ShowLeaderBoard(string Board)
    {
        PlayService.Instance.ShowLeaderboard(Board);

    }
    public void Logout()
    {
        PlayService.Instance.SignOut();
        signedout = true;
        Title_CanvasLink.isSignedOut();
    }
    private void OnLoginSuccess()
    {
        if(signedout)
        {
            signedout = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        Load();
        StartCoroutine(KeepCheckingAvatar());
        Title_CanvasManager.instance.blocker.SetActive(false);
        AccountNameText.text = Social.localUser.userName;
    }

    private IEnumerator KeepCheckingAvatar()
    {
        float secondsOfTrying = 10;
        float secondsPerAttempt = 0.2f;
        while (secondsOfTrying > 0)
        {
            if (Social.localUser.image != null)
            {
                //Debug.Log("IMAGE FOUND");
                Texture2D tmpImage = Social.localUser.image; // ProfilePic
                Title_CanvasLink.ChangeProfileImage(tmpImage);

                break;
            }
            //Debug.Log("IMAGE NOT FOUND");
            secondsOfTrying -= secondsPerAttempt;
            yield return new WaitForSeconds(secondsPerAttempt);
        }
    }

    private void OnLoginFail()
    {
        Title_CanvasManager.instance.blocker.SetActive(false);
    }
}
