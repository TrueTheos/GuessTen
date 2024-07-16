using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class GS_CanvasManager : MonoBehaviour
{

    public static GS_CanvasManager instance;

    [SerializeField] public GameObject SettingsMenu, ReturnMainMenu, LoadingMenu,Keyboard, GameOverMenu,WinMenu, HintsGO, KeysGO;
    [SerializeField] private Slider LoadingSlider, MusicSlider, FX_Slider;
    [SerializeField] private TMP_Text MusicSliderPercent, FX_SliderPercent;
    [SerializeField] private CanvasGroup Luckys,Keys;

    [SerializeField] private Transform[] FrameTransforms;
    [SerializeField] private Transform[] KeyboardTransforms;
    [SerializeField] public GameObject Border, EnterHL, BackHL, LivesCounter;

    private void Awake() 
    {
        instance = this;
    }

    private void Start()
    {
        Border.transform.DOLocalMoveY(4000f,0.1f);
        LivesCounter.transform.DOLocalMoveY(-4000f, 0.1f);


        for (int i = 0; i < FrameTransforms.Length; i++)
        {
            FrameTransforms[i].DOLocalMoveY(4000f, 0.1f);         //Title Drop Animation 
        }

        for (int i = 0; i < KeyboardTransforms.Length; i++)
        {
            KeyboardTransforms[i].DOLocalMoveY(-2000f, 0.1f);         //Title Drop Animation 
        }

        MusicSlider.onValueChanged.AddListener((v) => {
            MusicSliderPercent.text = (Mathf.FloorToInt(v * 100).ToString("0") + '%');    //Music Slider
        });
        FX_Slider.onValueChanged.AddListener((v) => {
            FX_SliderPercent.text = (Mathf.FloorToInt(v * 100).ToString("0") + '%');    //FX Slider
        });


        if (!PlayerPrefs.HasKey("MusicVolume") && !PlayerPrefs.HasKey("FXVolume"))      //Get Player Sound Prefs
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            PlayerPrefs.SetFloat("FXVolume", 1);
            Load();
        }
        else
        {
            Load();
        }

        DropBounceAnimation(KeyboardTransforms, 1f);
        DropBounceAnimation(FrameTransforms, 1f);
        BorderLivesAnimation();

    }

    public void ReturnButton()
    {
        MusicTransition.instance.OnButtonClick();//Play Effect

        ReturnMainMenu.SetActive(true);
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void ConfirmQuitButton(int sceneIndex)
    {
        MusicTransition.instance.OnButtonClick();//Play Effect

        Keyboard.SetActive(false);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
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
    public void SettingsButton()
    {
        MusicTransition.instance.OnButtonClick();//Play Effect
        SettingsMenu.SetActive(true);
    }
    public void RetryButton(int sceneIndex)
    {
        MusicTransition.instance.OnButtonClick();//Play Effect

        StartCoroutine(LoadAsynchronously(sceneIndex));
        GameOverMenu.SetActive(false);
    }

    public void GameOverDialog()
    {
        Keyboard.SetActive(false);
        HintsGO.SetActive(false);
        KeysGO.SetActive(false);
        GameOverMenu.SetActive(true);
    }
    public void GameOverCHALLENGEDialog()
    {
        Keyboard.SetActive(false);
        HintsGO.SetActive(false);
        GameOverMenu.SetActive(true);
    }
    public void ChallengeWinDialog()
    {
        Keyboard.SetActive(false);
        HintsGO.SetActive(false);
        WinMenu.SetActive(true);
    }

    public void LuckyCounterAlpha(int hints)
    {
        if (hints > 0)
        {
            Luckys.alpha = 1f;
        }
        else
            Luckys.alpha = 0.5f;
    }
    public void KeysCounterAlpha(int hints)
    {
        //Debug.Log("Keys Loaded: "+hints);
        if (hints > 0)
        {
            Keys.alpha = 1f;
        }
        else
            Keys.alpha = 0.5f;
    }

    public void EnterHighlight(int IO)
    {
        if (IO == 0)
        {
            EnterHL.SetActive(false);
        }
        else
            EnterHL.SetActive(true);
    }
    public void BackspaceHighlight(int IO)
    {
        if (IO == 0)
        {
            BackHL.SetActive(false);
        }
        else
            BackHL.SetActive(true);
    }

    public void UpdateChallengeTime()
    {
        CloudSaveManager.instance.State._ChlTime = DateTime.Now;
        CloudSaveTest.instance.Save();
    }

    //---------------Animations--------------------------------------------------------

    public void DropBounceAnimation(Transform[] transform, float Duration)
    {
        for (int i = 0; i < transform.Length; i++)
        {
            transform[i].DOLocalMoveY(0f, Duration)
            .SetEase(Ease.OutSine)
            .SetDelay(0.05f * i);
        }
    }
    public void BorderLivesAnimation()
    {
        Border.transform.DOLocalMoveY(656f, 2).SetEase(Ease.OutSine).SetDelay(2f);
        LivesCounter.transform.DOLocalMoveY(8.75f, 2).SetEase(Ease.OutSine).SetDelay(2f);

    }
    public void HintButtonsAnimation(bool chal)
    {
        if (chal)
        {
            HintsGO.SetActive(true);
            HintsGO.transform.DOScale(1,2).SetDelay(2).SetEase(Ease.OutBounce);
        }else
        {
            HintsGO.SetActive(true);
            KeysGO.SetActive(true);
            HintsGO.transform.DOScale(1, 2).SetDelay(2).SetEase(Ease.OutBounce);
            KeysGO.transform.DOScale(1, 2).SetDelay(2).SetEase(Ease.OutBounce);
        }

    }


    IEnumerator BorderScale(RectTransform Borderwidth, int endSize, bool increase)
    {
        int i = 0;
        if (increase)//increase Size
        {
            i=2;
            while (Borderwidth.sizeDelta.x < endSize)
            {
                Borderwidth.sizeDelta = new Vector2(Borderwidth.sizeDelta.x + i, 150);
                yield return Borderwidth;
            }
        }
        else//Decrease Size
        {
            i=-4;
            while (Borderwidth.sizeDelta.x > endSize)
            {
                Borderwidth.sizeDelta = new Vector2(Borderwidth.sizeDelta.x + i, 150);
                yield return Borderwidth;
            }
        }

        
//            yield return null;
        
    }
    public void MoveBorder(int level)
    {
        RectTransform Borderwidth = Border.GetComponent<RectTransform>();

        switch (level)
        {
            case 0:
                Border.transform.DOLocalMoveY(656f, 1).SetEase(Ease.OutSine);
                StartCoroutine(BorderScale(Borderwidth, 450, false));
                break;
            case 1:
                Border.transform.DOLocalMoveY(552f, 1).SetEase(Ease.OutSine);
                break;
            case 2:
                Border.transform.DOLocalMoveY(448f, 1).SetEase(Ease.OutSine);
                break;
            case 3:
                Border.transform.DOLocalMoveY(344f, 1).SetEase(Ease.OutSine);
                StartCoroutine(BorderScale(Borderwidth, 550, true));
                break;
            case 4:
                Border.transform.DOLocalMoveY(240f, 1).SetEase(Ease.OutSine);
                break;
            case 5:
                Border.transform.DOLocalMoveY(136f, 1).SetEase(Ease.OutSine);
                break;
            case 6:
                Border.transform.DOLocalMoveY(32f, 1).SetEase(Ease.OutSine);
                StartCoroutine(BorderScale(Borderwidth, 650, true));
                break;
            case 7:
                Border.transform.DOLocalMoveY(-72f, 1).SetEase(Ease.OutSine);
                break;
            case 8:
                Border.transform.DOLocalMoveY(-176f, 1).SetEase(Ease.OutSine);
                break;
            case 9:
                Border.transform.DOLocalMoveY(-280f, 1).SetEase(Ease.OutSine);
                StartCoroutine(BorderScale(Borderwidth, 750, true));
                break;

        }
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

}
