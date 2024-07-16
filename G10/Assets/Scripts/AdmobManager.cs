using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;

    string App_ID = "ca-app-pub-2567945786957931~6430708640";

    string Interstitial_Ad_ID = "ca-app-pub-2567945786957931/2079049020";
    string Interstitial_Ad_TEST = "ca-app-pub-3940256099942544/1033173712";

    private InterstitialAd interstitial;

    private void Awake()
    { 

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        MobileAds.Initialize(App_ID => {
            //Debug.Log("Initialized MobileAds");
        });
        RequestInterstitial();
        //DontDestroyOnLoad(this);
    }

    private void RequestInterstitial()
    {
    #if UNITY_ANDROID
            string adUnitId = Interstitial_Ad_ID;
    #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
            string adUnitId = "unexpected_platform";
    #endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdDidRecordImpression += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
        MusicTransition.instance.audioSourceMusic.Pause();

    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        MusicTransition.instance.audioSourceMusic.Play();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }



    public void GameOverAd()
    {
        if (!CloudSaveManager.Instance.State.AdFree)
        {
            if ((Stats_Manager.instance.GamesPlayed % 5) == 0)
            {
                //Debug.Log("Play Ad");
                if (this.interstitial.IsLoaded())
                {
                    this.interstitial.Show();
                    RequestInterstitial();
                }
            }
            //else
                //Debug.Log("Waiting for more games to be played before ad");
        }
        //else
            //Debug.Log("Ad Removal Purchased Dont Play Ads");
    }

}
