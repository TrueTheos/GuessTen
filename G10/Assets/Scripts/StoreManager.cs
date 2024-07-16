using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Text;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    [SerializeField] public int PlayerCoins, PlayerKeys;
    [SerializeField] public string UnlockedKeys, UnlockedBGs;
    [SerializeField] public Button[] KeysSelectButton, KeysBuyButton, BGSelectButton, BGBuyButton;
    [SerializeField] public TMP_Text[] UnlockedKeysEquipText, UnlockedBGsText, KeysCostText, BGCostText;
    [SerializeField] public TMP_Text PlayerCoinsText, PlayerKeysText;
    public GameObject adRemButton;
    public bool adfree;
    public List<GameObject> backgrounds;
    public GameObject equippedBGGO;
    public int equippedBGid;
    public List<Sprite> keyboardColors;
    public int currentlyUsedKeyboardIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void Init()
    {
        LoadPlayerUnlockedKeys(UnlockedKeys);
        LoadPlayerUnlockedBGs(UnlockedBGs);
        PlayerCoinsText.text = PlayerCoins.ToString();
        LoadAdBool(adfree);
    }

    public void LoadPlayerCoinsKeys()
    {
        if (PlayerCoins < CloudSaveManager.instance.State.Coins)
        {
            PlayerCoins = CloudSaveManager.instance.State.Coins;
            PlayerCoinsText.text = PlayerCoins.ToString();
            PlayerKeys = CloudSaveManager.instance.State.Keys;
            PlayerKeysText.text = PlayerKeys.ToString();
        }
    }

    public void LoadAdBool(bool ads)
    {
        //string[] tmp = str.Split(char.Parse("|"));
        if (ads)
        {
            adfree = true;
            Debug.Log("Ad Free");
            adRemButton.SetActive(false);
            CloudSaveTest.instance.Save();
        }
        else
            adfree = false;
            adRemButton.SetActive(true);
            Debug.Log("Free Version With Ads");
            CloudSaveTest.instance.Save();

    }

    public void UpdateCoins(int amount)
    {
        PlayerCoins += amount;
        try { PlayerCoinsText.text = PlayerCoins.ToString(); }catch { }
        CloudSaveTest.instance.Save();
    }
    public void UpdateKeys(int amount)
    {
        PlayerKeys += amount;
        PlayerKeysText.text = PlayerKeys.ToString();
        CloudSaveTest.instance.Save();
    }

    public void LoadPlayerUnlockedKeys(string Keys)
    {
        UnlockedKeys = Keys;
        string[] tmp2 = UnlockedKeys.Split(char.Parse("."));
        for (int i = 0; i < tmp2.Length; i++)
        {
            int KeyCheck = int.Parse(tmp2[i]);

            switch (KeyCheck)
            {
                case 0: //to buy
                    KeysSelectButton[i].interactable = false;
                    KeysBuyButton[i].interactable = true;
                    UnlockedKeysEquipText[i].text = "";
                    KeysCostText[i].text = "10,000";
                    break;
                case 1: //owned
                    KeysSelectButton[i].interactable = true;
                    KeysBuyButton[i].interactable = false;
                    UnlockedKeysEquipText[i].text = "";
                    KeysCostText[i].text = "OWNED";
                    break;
                case 2: //to owned and equipped
                    KeysSelectButton[i].interactable = false;
                    KeysBuyButton[i].interactable = false;
                    UnlockedKeysEquipText[i].text = "EQUIPPED";
                    KeysCostText[i].text = "OWNED";
                    currentlyUsedKeyboardIndex = i;
                    break;
            }
        }
    }

    public void LoadPlayerUnlockedBGs(string BGs)
    {
        UnlockedBGs = BGs;
        string[] tmp2 = UnlockedBGs.Split(char.Parse("."));
        for (int i = 0; i < tmp2.Length; i++)
        {
            int KeyCheck = int.Parse(tmp2[i]);

            switch (KeyCheck)
            {
                case 0: //to buy
                    BGSelectButton[i].interactable = false;
                    BGBuyButton[i].interactable = true;
                    UnlockedBGsText[i].text = "";
                    BGCostText[i].text = "100,000";
                    break;
                case 1: //owned
                    BGSelectButton[i].interactable = true;
                    BGBuyButton[i].interactable = false;
                    UnlockedBGsText[i].text = "";
                    BGCostText[i].text = "OWNED";
                    break;
                case 2: //to owned and equipped
                    BGSelectButton[i].interactable = false;
                    BGBuyButton[i].interactable = false;
                    UnlockedBGsText[i].text = "EQUIPPED";
                    BGCostText[i].text = "OWNED";
                    try { Destroy(equippedBGGO.gameObject); } catch { }
                    equippedBGGO = Instantiate(backgrounds[i]);
                    equippedBGid = i;
                    break;
            }
        }
    }

    public void TriggerButton(int id)
    {
        if (int.Parse(UnlockedBGs.Split(char.Parse("."))[id]) == 0)
        {
            if (PlayerCoins >= 100000)
            {
                UpdateCoins(-100000);
                BGSelectButton[id].interactable = true;
                BGBuyButton[id].interactable = false;
                UnlockedBGsText[id].text = "";
                BGCostText[id].text = "OWNED";
                UpdateBGText(id, 1);
            }
        }
        else if (int.Parse(UnlockedBGs.Split(char.Parse("."))[id]) == 1)
        {
            for (int i = 0; i < UnlockedBGs.Split(char.Parse(".")).Length; i++)
            {
                if (int.Parse(UnlockedBGs.Split(char.Parse("."))[i]) == 2)
                {
                    UnlockedBGsText[i].text = "";
                    BGSelectButton[i].interactable = true;
                    UpdateBGText(i, 1);
                }
            }

            try { Destroy(equippedBGGO.gameObject); } catch { }
            equippedBGGO = Instantiate(backgrounds[id]);
            equippedBGid = id;

            UnlockedBGsText[id].text = "EQUIPPED";
            BGSelectButton[id].interactable = false;          
            UpdateBGText(id, 2);
        }

        CloudSaveTest.instance.Save();
    }

    public void TriggerItem(int id)
    {
        if(int.Parse(UnlockedKeys.Split(char.Parse("."))[id]) == 0)
        {
            if(PlayerCoins >= 10000)
            {
                UpdateCoins(-10000);
                KeysSelectButton[id].interactable = true;
                KeysBuyButton[id].interactable = false;
                UnlockedKeysEquipText[id].text = "";
                KeysCostText[id].text = "OWNED";
                UpdateKeysText(id, 1);
            }
        }
        else if (int.Parse(UnlockedKeys.Split(char.Parse("."))[id]) == 1)
        {
            for (int i = 0; i < UnlockedKeys.Split(char.Parse(".")).Length; i++)
            {
                if (int.Parse(UnlockedKeys.Split(char.Parse("."))[i]) == 2)
                {
                    UnlockedKeysEquipText[i].text = "";
                    KeysSelectButton[i].interactable = true;
                    UpdateKeysText(i, 1);
                }
            }

            UnlockedKeysEquipText[id].text = "EQUIPPED";
            KeysSelectButton[id].interactable = false;
            currentlyUsedKeyboardIndex = id;
            UpdateKeysText(id, 2);
        }

        CloudSaveTest.instance.Save();
    }

    private void UpdateKeysText(int index, int value)
    {
        int i = index * 2;
        string v = value.ToString();

        StringBuilder sb = new StringBuilder(UnlockedKeys);
        sb[i] = v[0];
        UnlockedKeys = sb.ToString();
        if (!UnlockedKeys.Contains("0"))
        {
            Achievements.instance.GrantCosmeticAchievements(false);
        }
        CloudSaveManager.instance.UpdateState();
        CloudSaveTest.instance.Save();
    }

    private void UpdateBGText(int index, int value)
    {
        int i = index * 2;
        string v = value.ToString();

        StringBuilder sb = new StringBuilder(UnlockedBGs);
        sb[i] = v[0];
        UnlockedBGs = sb.ToString();
        if (!UnlockedBGs.Contains("0"))
        {
            Achievements.instance.GrantCosmeticAchievements(true);
        }
        CloudSaveManager.instance.UpdateState();
        CloudSaveTest.instance.Save();
    }
}
