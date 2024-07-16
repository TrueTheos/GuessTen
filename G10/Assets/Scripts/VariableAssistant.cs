using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VariableAssistant : MonoBehaviour
{
    public Button[] KeysSelectButton, KeysBuyButton, BGSelectButton, BGBuyButton;
    public TMP_Text[] UnlockedKeysEquipText, UnlockedBGsText, KeysCostText, BGCostText;
    public TMP_Text PlayerCoinsText, PlayerKeysText;
    public GameObject adRemButton;

    public Title_CanvasManager Title_CanvasLink;
    public TMP_Text AccountNameText;

    public TMP_Text WordsGuessed_TMP, LettersPlayed_TMP, LuckyGuesses_TMP, ChallangesWon_TMP, GamesPlayed_TMP, Highlevel_TMP, N_HighestScore_TMP, C_HighScore_TMP;

    public static VariableAssistant Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Setup()
    {
        StoreManager sm = StoreManager.instance;
        CloudSaveTest cst = CloudSaveTest.instance;

        sm.KeysSelectButton = KeysSelectButton;
        sm.KeysBuyButton = KeysBuyButton;
        sm.BGSelectButton = BGSelectButton;
        sm.BGBuyButton = BGBuyButton;
        sm.UnlockedKeysEquipText = UnlockedKeysEquipText;
        sm.UnlockedBGsText = UnlockedBGsText;
        sm.KeysCostText = KeysCostText;
        sm.BGCostText = BGCostText;
        sm.PlayerCoinsText = PlayerCoinsText;
        sm.PlayerKeysText = PlayerKeysText;
        sm.adRemButton = adRemButton;

        cst.Title_CanvasLink = Title_CanvasLink;
        cst.AccountNameText = AccountNameText;

        Stats_Manager.instance.WordsGuessed_TMP = WordsGuessed_TMP;
        Stats_Manager.instance.LettersPlayed_TMP = LettersPlayed_TMP;
        Stats_Manager.instance.LuckyGuesses_TMP =  LuckyGuesses_TMP;
        Stats_Manager.instance.ChallangesWon_TMP = ChallangesWon_TMP;
        Stats_Manager.instance.GamesPlayed_TMP = GamesPlayed_TMP;
        Stats_Manager.instance.Highlevel_TMP = Highlevel_TMP;
        Stats_Manager.instance.N_HighestScore_TMP = N_HighestScore_TMP;
        Stats_Manager.instance.C_HighScore_TMP = C_HighScore_TMP;
    }

    public void TriggerButton(int id)
    {
        StoreManager.instance.TriggerButton(id);
    }

    public void TriggerItem(int id)
    {
        StoreManager.instance.TriggerItem(id);
    }
}
