using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterButton : MonoBehaviour, IPointerClickHandler
{
    public char letter;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.gameObject.GetComponent<BoxCollider2D>().enabled)
        {
            MusicTransition.instance.OnKeyClick();
            GameManager.instance.AddLetter(letter);
        }
    }
}
