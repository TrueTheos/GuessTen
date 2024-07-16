using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FrameButton : MonoBehaviour, IPointerClickHandler
{
    public int wordIndex, myIndex;
    public GameObject QuestionMark;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManager.instance.usingHint) return;
        bool tileExists = GameManager.instance.wordsObjects[wordIndex].slots[myIndex].gameObject.transform.childCount == 0 ? false : true;
        if (!tileExists)
        {
            if (!HintController.instance.HIO)
            {
                Keys_Controller.instance.numberOfKeys--;
                Keys_Controller.instance.keyhintsText.text = Keys_Controller.instance.numberOfKeys.ToString();
                StoreManager.instance.UpdateKeys(-1);
                Keys_Controller.instance.KeyHint = false;
                GameManager.instance.RevealLetter(myIndex, wordIndex);
                Keys_Controller.instance.CancelIcon.SetActive(false);
                HintController.instance.CancelIcon.SetActive(false);
                Keys_Controller.instance.FinishedHintReveal();

            }
            else
            {
                GameManager.instance.RevealLetter(myIndex, wordIndex);
                HintController.instance.FinishedHintReveal();
                Keys_Controller.instance.CancelIcon.SetActive(false);
            }
        }
        else if (GameManager.instance.wordsObjects[wordIndex].slots[myIndex].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite.name != GameManager.instance.greenTile.name)
        {
            if (!HintController.instance.HIO)
            {
                Keys_Controller.instance.numberOfKeys--;
                Keys_Controller.instance.keyhintsText.text = Keys_Controller.instance.numberOfKeys.ToString();
                StoreManager.instance.UpdateKeys(-1);
                Keys_Controller.instance.KeyHint = false;
                GameManager.instance.RevealLetter(myIndex, wordIndex);
                Keys_Controller.instance.CancelIcon.SetActive(false);
                HintController.instance.CancelIcon.SetActive(false);
                Keys_Controller.instance.FinishedHintReveal();
            }
            else
            {
                GameManager.instance.RevealLetter(myIndex, wordIndex);
                HintController.instance.FinishedHintReveal();
                Keys_Controller.instance.CancelIcon.SetActive(false);

            }
        }
    }
}
