using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControl : MonoBehaviour
{
    public List<GameObject> backgrounds;

    private int currentBackground = 0;

    private void SetBackground(int index)
    {
        foreach (GameObject background in backgrounds)
        {
            background.SetActive(false);
        }
        backgrounds[index].SetActive(true);
    }

    private void Start()
    {
        SetBackground(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentBackground++;
            if (currentBackground >= backgrounds.Count)
            {
                currentBackground = 0;
            }
            SetBackground(currentBackground);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentBackground--;
            if (currentBackground < 0)
            {
                currentBackground = backgrounds.Count - 1;
            }
            SetBackground(currentBackground);
        }
    }
}
