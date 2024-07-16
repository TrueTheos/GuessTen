using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class Title_CanvasManager : MonoBehaviour
{
    public static Title_CanvasManager instance;

    [SerializeField] private GameObject SettingsMenu, MainMenu,LoadingMenu,StatsMenu,LeaderBoardMenu,StoreMenu,SignIOBar,AchievementsButton,SignInButton,SignOutButton;
    [SerializeField] public Slider LoadingSlider, MusicSlider, FX_Slider;
    [SerializeField] private TMP_Text MusicSliderPercent, FX_SliderPercent;
    [SerializeField] private Transform[] MainTitleTransform, StoreTitleTransform, StoreCellsTransform;
    [SerializeField] private Ease animEase;
    [SerializeField] public GameObject[] S_Clovers;
    [SerializeField] public TMP_Text logText,ChlTimeText;
    [SerializeField] public GameObject ProfileImage, ChlLock;
    [SerializeField] public Sprite DefaultPlayerIcon;
    [SerializeField] public GameObject blocker;
    public Button ChallengeButton;

    public DateTime _ChlTime;
    public TimeSpan _ChlTimeLeft;

    private int S_C_Count;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChallengeButton.interactable = false;
        ChlLock.SetActive(true);
        ChlTimeText.alpha = 0;

        S_C_Count = 0;
        if (!PlayerPrefs.HasKey("FirstGame"))
            PlayerPrefs.SetInt("FirstGame", 0);

        MusicSlider.onValueChanged.AddListener((v) => {
            MusicSliderPercent.text = (Mathf.FloorToInt(v * 100).ToString("0") + '%');    //Music Slider
        });
        FX_Slider.onValueChanged.AddListener((v) => {
            FX_SliderPercent.text = (Mathf.FloorToInt(v * 100).ToString("0") + '%');    //FX Slider
        });

        if (!PlayerPrefs.HasKey("MusicVolume") && !PlayerPrefs.HasKey("FXVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            PlayerPrefs.SetFloat("FXVolume", 1);
            Load();
        } //Player Pref Volume
        else
        {
            Load();

        }



        MainMenu.transform.localScale = new Vector3(0, 0, 0);   //MainMenu setup Animation
        for (int i = 0; i < S_Clovers.Length; i++)
        {
            S_Clovers[i].transform.localScale = new Vector3(0, 0, 0);   //MainMenu setup Animation
        }
        for (int i = 0; i < MainTitleTransform.Length; i++)
        {
            MainTitleTransform[i].DOLocalMoveY(1000f, 0.1f);         //Main Title Drop Animation Setup 
        }
        for (int i = 0; i < StoreTitleTransform.Length; i++)
        {
            StoreTitleTransform[i].DOScale(0f, 0.2f);         //Store Title Animation Setup 
        }
        for (int i = 0; i < StoreCellsTransform.Length; i++)
        {
            StoreCellsTransform[i].DOScale(0f, 0.2f);         //Store Title Animation Setup 
        }

        DropAnimation();
        MainMenuAnimation();
        CloudSaveTest.instance.Login();

    }

    public void Update()
    {

        if (S_C_Count == 10)
        {
            SC_ScaleAnimation();
            S_C_Count = 0;
        }

        if (ChlTimeText.alpha ==1)
        {
            _ChlTimeLeft = DateTime.Now - _ChlTime;
            if ((_ChlTimeLeft <= TimeSpan.FromHours(6)))
            {
                if ((TimeSpan.FromHours(6) - _ChlTimeLeft).Hours == 0)
                    ChlTimeText.text = (TimeSpan.FromHours(6) - _ChlTimeLeft).ToString(@"mm\M\:ss\S");// + " : " + (60 -_ChlTimeLeft.Seconds).ToString(); //ToString(@"mm\:ss");
                else
                    ChlTimeText.text = (TimeSpan.FromHours(6) - _ChlTimeLeft).ToString(@"hh\H\:mm\M");
            }
            else if ((_ChlTimeLeft >= TimeSpan.FromHours(6)))
            {
                ChallengeButton.interactable = true;
                ChlLock.SetActive(false);
            }
            else
            {
                ChallengeButton.interactable = false;
                ChlLock.SetActive(true);
            }
        }
        //if (PlayerPrefs.GetInt("FirstGame")>0)
        //{
        //    ChlTimeText.alpha = 1;
        //    _ChlTimeLeft = DateTime.Now - _ChlTime;
        //    if ((_ChlTimeLeft <= TimeSpan.FromHours(1)))
        //    {
        //        if ((TimeSpan.FromHours(1) - _ChlTimeLeft).Hours == 0)
        //            ChlTimeText.text = (TimeSpan.FromHours(1) - _ChlTimeLeft).ToString(@"mm\M\:ss\S");// + " : " + (60 -_ChlTimeLeft.Seconds).ToString(); //ToString(@"mm\:ss");
        //        else
        //            ChlTimeText.text = (TimeSpan.FromHours(1) - _ChlTimeLeft).ToString(@"hh\H\:mm\M");
        //    }
        //    else if ((_ChlTimeLeft >= TimeSpan.FromHours(1)))
        //    {
        //        ChallengeButton.interactable = true;
        //        ChlLock.SetActive(false);
        //    }
        //    else
        //    {
        //        ChallengeButton.interactable = false;
        //        ChlLock.SetActive(true);
        //    }
        //}
        //Debug.Log(_ChlTime);
        //Debug.Log("\n\n " + CloudSaveManager.instance.State._ChlTime + "\n\n");
        //if (_ChlTimeLeft <= TimeSpan.FromMinutes(5))
        //{
        //    if ((TimeSpan.FromMinutes(5) - _ChlTimeLeft).Hours == 0)
        //        ChlTimeText.text = (TimeSpan.FromMinutes(5) - _ChlTimeLeft).ToString(@"mm\M\:ss\S");// + " : " + (60 -_ChlTimeLeft.Seconds).ToString(); //ToString(@"mm\:ss");
        //    else
        //        ChlTimeText.text = (TimeSpan.FromMinutes(5) - _ChlTimeLeft).ToString(@"hh\H\:mm\M");
        //}

    }
    // Start is called before the first frame update
    public void PlayButton(int sceneIndex)
    {
        if (sceneIndex == 1)
        {
            if (PlayerPrefs.GetInt("FirstGame") ==0) //Tutorial Initial Play
            {
                MusicTransition.instance.OnButtonClick();//Play Effect

                SettingsMenu.SetActive(false);
                MainMenu.SetActive(false);
                StartCoroutine(TitleAnimationFall(sceneIndex + 2));

            }
            else
            {
                MusicTransition.instance.OnButtonClick();//Play Effect

                SettingsMenu.SetActive(false);
                MainMenu.SetActive(false);
                StartCoroutine(TitleAnimationFall(sceneIndex));
            }
        }
        if(sceneIndex == 2)
        {
            SettingsMenu.SetActive(false);
            MainMenu.SetActive(false);
            _ChlTime = DateTime.Now;
            CloudSaveTest.instance.Save();
            StartCoroutine(TitleAnimationFall(sceneIndex));
        }
    }
    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        LoadingMenu.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            LoadingSlider.value = progress;
            
            yield return null;
        }
    }

    public void LeaderBoardsButton()
    {
        MusicTransition.instance.OnButtonClick();//Play Effect

        MainMenu.SetActive(false);
        LeaderBoardMenu.SetActive(true);
    }

    public void SettingsButton()
    {
        MusicTransition.instance.OnButtonClick();//Play Effect

        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }
    public void StatsButton()
    {
        MusicTransition.instance.OnButtonClick();//Play Effect

        MainMenu.SetActive(false);
        StatsMenu.SetActive(true);
    }
    public void PlayerAccount()
    {
        MusicTransition.instance.OnButtonClick();//Play Effect
        if (SignIOBar.activeSelf == true)
        {
            SignIOBar.SetActive(false);
        }
        else
            SignIOBar.SetActive(true);
    }

    public void isSignedIn()
    {
        AchievementsButton.SetActive(true);
        SignOutButton.SetActive(true);
        SignInButton.SetActive(false);
    }
    public void isSignedOut()
    {
        AchievementsButton.SetActive(false);
        SignOutButton.SetActive(false);
        SignInButton.SetActive(true);
        ProfileImage.GetComponent<Image>().sprite = DefaultPlayerIcon;
        StoreManager.instance.PlayerCoins = 0;
        StoreManager.instance.PlayerKeys = 0;
        StoreManager.instance.PlayerKeysText.text = "0";
        StoreManager.instance.PlayerCoinsText.text = "0";
        Stats_Manager.instance.StatsSignOut();
    }


    public void ExitButton()
    {
        MusicTransition.instance.OnButtonClick();//Play Effect

        Application.Quit();
    }

    public void StoreOpenButton()
    {
        MusicTransition.instance.OnButtonClick();//Play Effect
        for (int i = 0; i < MainTitleTransform.Length; i++)
        {
            MainTitleTransform[i].DOScale(0f, 1);
        }

        MainMenu.SetActive(false);
        StoreMenu.SetActive(true);
        StoreTitleAnimation();
        StoreCellsAnimation();
        MainMenu.transform.localScale = new Vector3(0, 0, 0);   //MainMenu setup Animation

    }
    public void StoreCloseButton()   //Store Close Animations
    {
        for (int i = 0; i < StoreCellsTransform.Length; i++)
        {
            StoreCellsTransform[i].DOScale(0f, 1);
        }
        for (int i = 0; i < StoreTitleTransform.Length; i++)
        {
            StoreTitleTransform[i].DOScale(0f, 1);
        }
        for (int i = 0; i < MainTitleTransform.Length; i++)
        {
            MainTitleTransform[i].DOScale(1f, 1)
            .SetEase(animEase,3)
            .SetDelay(0.1f * i);
        }
        MainMenu.SetActive(true);
        StoreMenu.SetActive(false);
        MainMenuAnimation();
    }
    public void SetChlTime(DateTime DT)
    {
        _ChlTime = DT;
        _ChlTimeLeft = DateTime.Now - _ChlTime;

        if (PlayerPrefs.GetInt("FirstGame") != 0)
        {
            if (_ChlTimeLeft <= TimeSpan.FromHours(6))
            {
                ChlLock.SetActive(true);
                ChallengeButton.interactable = false;
                ChlTimeText.alpha = 1;
            }
            else
            {
                ChlLock.SetActive(false);
                ChallengeButton.interactable = true;
                ChlTimeText.alpha = 1;
            }
        }
        else
        {
            ChlLock.SetActive(true);
            ChallengeButton.interactable = false;
            ChlTimeText.alpha = 0;
            Debug.Log("Unity LOG:   Tutorial Not Played");
        }
    }
    //GPGS Button Links

    public void GPGSLoginButton()
    {
        CloudSaveTest.instance.Login();
    }
    public void GPGSLogoutButton()
    {
        CloudSaveTest.instance.Logout();
    }
    public void GPGSAchievementsButton()
    {
        CloudSaveTest.instance.ShowAchievements();
    }
    public void GPGSLeaderBoardsButton(string Boards)
    {
        CloudSaveTest.instance.ShowLeaderBoard(Boards);
    }


    //--------------Animations Scripts-----------------------------------------------------------------



    public void MainMenuAnimation()
    {
        MainMenu.transform.DOScale(1, 1f).SetEase(animEase, 2);

    }
    public void StoreTitleAnimation()   //Store Title Animation
    {
        for (int i = 0; i < StoreTitleTransform.Length; i++)
        {
            StoreTitleTransform[i].DOScale(1f, 1)
            .SetEase(animEase, 3)
            .SetDelay(0.2f * i);
        }
    }
    public void SCC() { S_C_Count += 1; }  
    
    public void SC_ScaleAnimation()   
    {
        Achievements.instance.DoGrantAchievement(GPGSIds.achievement_lucky_click);
        for (int i = 0; i < S_Clovers.Length; i++)
        {
            S_Clovers[i].SetActive(true);
            S_Clovers[i].transform.DOScale(1f, 3)
            .SetEase(animEase, 5)
            .SetDelay(0.1f * i);
            S_Clovers[i].transform.DOScale(0f, 3)
            .SetEase(animEase, 3)
            .SetDelay(3f);
        }
    }

    public void StoreCellsAnimation()   //Store Animations
    {
        for (int i = 0; i < StoreCellsTransform.Length; i++)
        {
            StoreCellsTransform[i].DOScale(1f, 2)
            .SetEase(animEase)
            .SetDelay(0.1f * i);
        }
    }

   
    public void DropAnimation()   //Drop Title on game start 
    {
        for (int i = 0; i < MainTitleTransform.Length; i++)
        {
            MainTitleTransform[i].DOLocalMoveY(0f, 2)
            .SetEase(animEase, 2)
            .SetDelay(0.1f * i);
        }
    }

    IEnumerator TitleAnimationFall(int sceneIndex)   //Drop Title on play button
    {
        for (int i = 0; i < MainTitleTransform.Length; i++)
        {
            MainTitleTransform[i].DOLocalMoveY(-3000f, 2)
            .SetEase(animEase)
            .SetDelay(0.1f * i);
        }
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(LoadAsynchronously(sceneIndex));
        yield return null;

    }




    //---------------Player Prefs------------------------------------------------------

    public void ChangeVolume()
    {
        MusicTransition.instance.FXMixer.audioMixer.SetFloat("music", Mathf.Log10(MusicSlider.value) * 30);
        MusicTransition.instance.FXMixer.audioMixer.SetFloat("fx", Mathf.Log10(FX_Slider.value) * 30);
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
        PlayerPrefs.SetFloat("FXVolume", FX_Slider.value);
    }
    private void Load()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        FX_Slider.value = PlayerPrefs.GetFloat("FXVolume");
        MusicTransition.instance.FXMixer.audioMixer.SetFloat("music", Mathf.Log10(MusicSlider.value) * 30);
        MusicTransition.instance.FXMixer.audioMixer.SetFloat("fx", Mathf.Log10(FX_Slider.value) * 30);
    }


    //---------------Google Play Service Cloud Data------------------------------------------------------
    public void ChangeProfileImage(Texture2D tex)
    {
        ProfileImage.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    //If Sign in succesful upload savedata 


}