using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public TMP_Text DialogueText;
    public GameObject HighlightFrame;
    public Animator MsgAnimator;
    public Button NextButton;
    public int sentenceInt;
    public GameObject[] locations;
    private string sentence;
    private void Start()
    {
        sentenceInt = 0;
        sentences = new Queue<string>();
        FindObjectOfType<DialogTrigger>().TriggerDialogue();
        DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        MsgAnimator.SetInteger("FrameInt", sentenceInt);
        //Debug.Log("Starting Conversation");
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (DialogueText.text != sentence)
        {
            StopAllCoroutines();
            DialogueText.text = sentence;
            return;
        }

        MsgAnimator.SetInteger("FrameInt", sentenceInt);

        if (sentenceInt < 14 && sentenceInt > 0)
        {
            if (sentenceInt != 14)
                locations[sentenceInt - 1].SetActive(true);
            if (sentenceInt >= 2 && locations[sentenceInt - 2].gameObject.transform.name != locations[sentenceInt - 1].gameObject.transform.name && sentenceInt < 14)
            {
                locations[sentenceInt - 2].SetActive(false);
            }

        }
        else if (sentenceInt == 14)
            locations[sentenceInt - 2].SetActive(false);

        sentenceInt++;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine("TypeSentence", sentence);
    }
    IEnumerator TypeSentence(string sentence)
    {
        //NextButton.interactable = false;
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return null;
        }
        //NextButton.interactable = true;
    }
    public void EndDialogue()
    {
        //Debug.Log("End Conversation");
        Destroy(NextButton.gameObject);
        PlayerPrefs.SetInt("FirstGame", 1);
        GameManager.instance.StartCoroutine("Timer");
    }
}
