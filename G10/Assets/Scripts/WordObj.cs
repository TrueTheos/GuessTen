using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordObj : MonoBehaviour
{
    public string word;
    public List<GameObject> slots = new List<GameObject>();

    public void Init()
    {
        for (int i = 0; i < word.Length - 1; i++)
        {
            slots.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }
}