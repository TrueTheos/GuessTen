using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class HintController : MonoBehaviour, IPointerClickHandler
{
    public static HintController instance;

    public int numberOfHints;
    public TextMeshProUGUI hintsText;
    public GameObject CancelIcon;
    public bool HIO;
    public GameObject QuestionMark;
     

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HIO = false;
        GS_CanvasManager.instance.LuckyCounterAlpha(numberOfHints);
    }

    public void AddHint()
    {
        numberOfHints++;
        hintsText.text = numberOfHints.ToString();
        GS_CanvasManager.instance.LuckyCounterAlpha(numberOfHints);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HIO)
        {
            if (numberOfHints > 0 && !GameManager.instance.usingHint)
            {
                CancelIcon.SetActive(true);
                GS_CanvasManager.instance.LuckyCounterAlpha(numberOfHints);
                GameManager.instance.usingHint = true;
                HIO = true;
                WhileUsingHintAnimation(HIO);
            }
        }
        else
        {
            GameManager.instance.usingHint = false;
            HIO = false;
            CancelIcon.SetActive(false);
            GS_CanvasManager.instance.LuckyCounterAlpha(numberOfHints);
            GameObject[] Spawns;
            Spawns = GameObject.FindGameObjectsWithTag("?");
            for (int i = 0; i < Spawns.Length; i++)
            {
                Destroy(Spawns[i]);
            }
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
    }
    public void FinishedHintReveal()
    {
        GameManager.instance.usingHint = false;
        HIO = false;
        CancelIcon.SetActive(false);
        numberOfHints--;
        hintsText.text = numberOfHints.ToString();
        GS_CanvasManager.instance.LuckyCounterAlpha(numberOfHints);
        GameObject[] Spawns;
        Spawns = GameObject.FindGameObjectsWithTag("?");

        for (int i = 0; i < 10; i++)
        {
            for (int y = 0; y < GameManager.instance.wordsObjects[i].slots.Count; y++)
            {

                GameManager.instance.wordsObjects[i].slots[y].gameObject.transform.DORewind();
                GameManager.instance.wordsObjects[i].slots[y].gameObject.transform.DOKill();

                if(GameManager.instance.wordsObjects[i].slots[y].transform.childCount > 1)
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
        for (int i = 0; i <10; i++)
        {
            for (int y = 0; y < GameManager.instance.wordsObjects[i].slots.Count; y++)
            {
                if(GameManager.instance.wordsObjects[i].slots[y].gameObject.transform.childCount == 0)
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
