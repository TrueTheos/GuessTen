using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;


public class Keys_Controller : MonoBehaviour, IPointerClickHandler
{
    public static Keys_Controller instance;

    public int numberOfKeys;
    public TextMeshProUGUI keyhintsText;
    public GameObject CancelIcon;
    public bool KeyHint;
    public GameObject QuestionMark;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        KeyHint = false;
        numberOfKeys = StoreManager.instance.PlayerKeys;
        keyhintsText.text = numberOfKeys.ToString();
        GS_CanvasManager.instance.KeysCounterAlpha(numberOfKeys);
    }

    public void AddKeyHints()
    {
        numberOfKeys++;
        CloudSaveManager.instance.State.Keys++;
        CloudSaveTest.instance.Save();
        keyhintsText.text = numberOfKeys.ToString();
        GS_CanvasManager.instance.KeysCounterAlpha(numberOfKeys);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (KeyHint == false)
        {
            if (numberOfKeys > 0 && !GameManager.instance.usingHint)
            {
                CancelIcon.SetActive(true);
                GS_CanvasManager.instance.KeysCounterAlpha(numberOfKeys);
                GameManager.instance.usingHint = true;
                KeyHint = true;
                WhileUsingHintAnimation(KeyHint);
                //blah blah blah
            }
        }
        else if(KeyHint == true)
        {
            GameManager.instance.usingHint = false;
            KeyHint = false;
            CancelIcon.SetActive(false);
            FinishedHintReveal();

        }
    }
    public void FinishedHintReveal()
    {
        GameManager.instance.usingHint = false;
        KeyHint = false;
        CancelIcon.SetActive(false);
        keyhintsText.text = numberOfKeys.ToString();
        GS_CanvasManager.instance.KeysCounterAlpha(numberOfKeys);
        GameObject[] Spawns;
        Spawns = GameObject.FindGameObjectsWithTag("?");
        for (int i = 0; i < 10; i++)
        {
            for (int y = 0; y < GameManager.instance.wordsObjects[i].slots.Count; y++)
            {

                GameManager.instance.wordsObjects[i].slots[y].gameObject.transform.DORewind();
                GameManager.instance.wordsObjects[i].slots[y].gameObject.transform.DOKill();

                if (GameManager.instance.wordsObjects[i].slots[y].transform.childCount > 1)
                {
                    GameManager.instance.wordsObjects[i].slots[y].transform.GetChild(1).transform.rotation = Quaternion.identity;
                }
            }
        }
        for (int i = 0; i < Spawns.Length; i++)
        {
            Destroy(Spawns[i]);
        }
    }
    public void WhileUsingHintAnimation(bool hint)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int y = 0; y < GameManager.instance.wordsObjects[i].slots.Count; y++)
            {
                if (GameManager.instance.wordsObjects[i].slots[y].gameObject.transform.childCount == 0)
                {
                    GameManager.instance.wordsObjects[i].slots[y].gameObject.transform.DOShakeRotation(5f, strength: new Vector3(0, 0, 10), vibrato: 0, fadeOut: false).SetDelay(y * 0.1f).SetLoops(-1);
                    Instantiate(QuestionMark, GameManager.instance.wordsObjects[i].slots[y].gameObject.transform);


                }
                else if (GameManager.instance.wordsObjects[i].slots[y].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite != GameManager.instance.greenTile)
                {
                    GameManager.instance.wordsObjects[i].slots[y].gameObject.transform.DOShakeRotation(5f, strength: new Vector3(0, 0, 10), vibrato: 0, fadeOut: false).SetDelay(y * 0.1f).SetLoops(-1);
                    Instantiate(QuestionMark, GameManager.instance.wordsObjects[i].slots[y].gameObject.transform);
                }
            }
        }
    }
}

