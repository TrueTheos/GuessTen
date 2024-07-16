using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
    public static MenuScene instance;
    public GameObject loadBlocker;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        VariableAssistant.Instance.Setup();
        StoreManager.instance.Init();
        CloudSaveTest.instance.Login();
        CloudSaveTest.instance.Save();
    }
}
