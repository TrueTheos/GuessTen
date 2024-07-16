using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        foreach (var item in GameManager.instance.buttons)
        {
            item.GetComponent<Image>().sprite = StoreManager.instance.keyboardColors[StoreManager.instance.currentlyUsedKeyboardIndex];
        }

        GameManager.instance.enterButton.GetComponent<Image>().sprite = StoreManager.instance.keyboardColors[StoreManager.instance.currentlyUsedKeyboardIndex];
        GameManager.instance.deleteButton.GetComponent<Image>().sprite = StoreManager.instance.keyboardColors[StoreManager.instance.currentlyUsedKeyboardIndex];

        GameObject bg = Instantiate(StoreManager.instance.backgrounds[StoreManager.instance.equippedBGid]);
    }
}
