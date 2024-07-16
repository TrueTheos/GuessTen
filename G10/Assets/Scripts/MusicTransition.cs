using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicTransition : MonoBehaviour
{
    public AudioClip first, second, onWrongGuess, onRightGuess, onKeyClick, onButtonClick, onGameOver, onWin, holeInOne; //Quality variable names
    public AudioSource audioSourceMusic,audioSourceFX;
    public AudioMixerGroup MusicMixer,FXMixer;
    public float FX_Volume;

    public static MusicTransition instance;

    private void Awake()
    {
        instance = this;
        //audioSourceMusic.Play();
    }

    public void TriggerMusicChange()
    {
        GetComponent<Animator>().SetTrigger("change");
    }

    public void ChangeMusic()
    {
        audioSourceMusic.clip = second;
        audioSourceMusic.Play();
    }

    public void OnWrongGuess()
    {
        audioSourceFX.clip = onWrongGuess;
        audioSourceFX.Play();
    }
    public void OnRightGuess()
    {
        audioSourceFX.clip = onRightGuess;
        audioSourceFX.Play();
    }
    public void OnKeyClick()
    {
        audioSourceFX.clip = onKeyClick;
        audioSourceFX.Play();
    }
    public void OnButtonClick()
    {
        audioSourceFX.clip = onButtonClick;
        audioSourceFX.Play();
       // audioSourceFX.PlayOneShot(onButtonClick, FX_Volume);
    }
    public void OnGameOver()
    {
        audioSourceFX.clip = onGameOver;
        audioSourceFX.Play();
    }
    public void OnWin()
    {
        audioSourceFX.clip = onWin;
        audioSourceFX.Play();
    }

    //public void SetFXVolume(float FX_Value)
    //{
    //    FX_Volume = FX_Value;
    //}

    public void PlayHoleInOne()
    {
        audioSourceFX.clip = holeInOne;
        audioSourceFX.Play();
    }
}
