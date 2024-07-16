using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class Achievements : MonoBehaviour
{
    public static Achievements instance;
   

    public void Awake()
    {
        instance = this;
    }

    public void DoGrantAchievement(string _achievement)
    {

        Social.LoadAchievements(achievements =>
        {
            //Debug.Log("Loaded Achivements" + achievements.Length);
            foreach (IAchievement ach in achievements)
            {
                if (ach.id == _achievement)
                {
                    //Debug.Log("Found Achievement");
                    if (!ach.completed)
                    {
                        Social.ReportProgress(_achievement, 100.00f, (bool success) =>
                        {

                            if (success)
                            {
                                Debug.Log(_achievement + "  Achivement was successfully updated");
                                StoreManager.instance.UpdateKeys(1);
                            }
                            else
                                Debug.Log(_achievement + "  Achivement failed to update success: ");

                        });
                        //Debug.Log("Found Achievement is completed:");

                    }
                    //else
                    //{
                        //Debug.Log("Found Achievement is Completed");
                    //}
                }
            }
        });


    }

    public void DoIncrementalAchievement(string _achievement, int increment)
    {
        PlayGamesPlatform platform = (PlayGamesPlatform)Social.Active;
        Social.LoadAchievements(achievements =>
        {
            //Debug.Log("Loaded Achivements" + achievements.Length);
            foreach (IAchievement ach in achievements)
            {
                if (ach.id == _achievement)
                {
                    //Debug.Log("Found Achievement");
                    if (!ach.completed)
                    {
                        platform.IncrementAchievement(_achievement, increment, (bool success) =>
                        {
                            if (success)
                            {
                                //Debug.Log(_achievement + "  Achivement was successfully stepped: " + increment + " times");
                                Social.LoadAchievements(achievementsupd =>
                                {
                                    foreach (IAchievement updatedach in achievementsupd)
                                    {
                                        if (updatedach.id == _achievement)
                                        {
                                            if (updatedach.completed)
                                            {
                                                StoreManager.instance.UpdateKeys(1);
                                                //Debug.Log("Achivement completed granting key");
                                            }
                                        }

                                    }
                                });
                            }
                            else
                                Debug.Log(_achievement + "Failed to Update");
                        });
                    }
                    else
                    {
                        Debug.Log("Found Achievement is Completed");
                    }
                }
            }
        });
    }

    public void DoRevealAchievement(string _achievement)
    {
        Social.ReportProgress(_achievement, 0.00f, (bool success) =>
        {
            if (success)
            {
                Debug.Log(_achievement + "  Achivement was successfully updated");
            }
            else
                Debug.Log(_achievement + "  Achivement failed to update");
        });
    }

    public void StepChallengerAchievements()
    {
        DoGrantAchievement(GPGSIds.achievement_the_challenger_i);
        DoIncrementalAchievement(GPGSIds.achievement_the_challenger_ii, 1);
        DoIncrementalAchievement(GPGSIds.achievement_the_challenger_iii, 1);
        DoIncrementalAchievement(GPGSIds.achievement_the_challenger_iv, 1);
        DoIncrementalAchievement(GPGSIds.achievement_the_challenger_v, 1);
    }
    public void GrantExpertAchievements(int Level)
    {
        switch (Level)
        {
            case 10:
                DoGrantAchievement(GPGSIds.achievement_the_expert_i);
                break;
            case 20:
                DoGrantAchievement(GPGSIds.achievement_the_expert_ii);
                break;
            case 30:
                DoGrantAchievement(GPGSIds.achievement_the_expert_iii);
                break;
            case 40:
                DoGrantAchievement(GPGSIds.achievement_the_expert_iv);
                break;
            case 50:
                DoGrantAchievement(GPGSIds.achievement_the_expert_v);
                break;
        }
    }
    public void StepGamePlayAchievements(int letters, int words, int luckys)
    {
        DoIncrementalAchievement(GPGSIds.achievement_100, 1);
        DoIncrementalAchievement(GPGSIds.achievement_lots_of_letters, letters);
        DoIncrementalAchievement(GPGSIds.achievement_word_master_i, words);
        DoIncrementalAchievement(GPGSIds.achievement_word_master_ii, words);
        DoIncrementalAchievement(GPGSIds.achievement_word_master_iii, words);
        DoIncrementalAchievement(GPGSIds.achievement_word_master_iv, words);
        DoIncrementalAchievement(GPGSIds.achievement_word_master_v, words);
        DoIncrementalAchievement(GPGSIds.achievement_lucky_guess_i, luckys);
        DoIncrementalAchievement(GPGSIds.achievement_lucky_guess_ii, luckys);
        DoIncrementalAchievement(GPGSIds.achievement_lucky_guess_iii, luckys);
        DoIncrementalAchievement(GPGSIds.achievement_lucky_guess_iv, luckys);
        DoIncrementalAchievement(GPGSIds.achievement_lucky_guess_v, luckys);

    }
    public void GrantCosmeticAchievements(bool BGs)
    {
        if (BGs)
        {
            DoGrantAchievement(GPGSIds.achievement_the_explorer);
        }
        else
            DoGrantAchievement(GPGSIds.achievement_the_stylist);
    }


}
