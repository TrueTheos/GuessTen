using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class GPGS_Manager_Test : MonoBehaviour
{

    public GameObject homeButton;
    private PlayGamesClientConfiguration clientConfiguration;
    public Text statusTxt;
    public Text descriptionText;
    // Start is called before the first frame update
    void Start()
    {

        ConfigurationGPGS();
        PlayGamesPlatform.Activate();
        SignIntoGPGS(SignInInteractivity.CanPromptOnce, clientConfiguration);


    }

     internal void ConfigurationGPGS()
     {
         clientConfiguration = new PlayGamesClientConfiguration.Builder().Build();
     }
    

    internal void SignIntoGPGS(SignInInteractivity interactivity, PlayGamesClientConfiguration configuration)
    {
        configuration = clientConfiguration;
        PlayGamesPlatform.InitializeInstance(configuration);
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(interactivity, (code) =>
        {
            statusTxt.text = "Authenticating...";
            if(code == SignInStatus.Success)
            {
                statusTxt.text = "Succesful login";
                descriptionText.text = "Hello   " + Social.localUser.userName + "You have ID:  " + Social.localUser.id;
                homeButton.SetActive(true);
            }
            else
            {
                statusTxt.text = "Failed to Authenticate";
                descriptionText.text = "Failed to Authenticate. Error:   " + code;
            }
        });
    }


    public void BasicSignInButton()
    {

        SignIntoGPGS(SignInInteractivity.CanPromptAlways, clientConfiguration);
        

    }

    public void BasicSignOutButton()
    {
        PlayGamesPlatform.Instance.SignOut();
        statusTxt.text = "Signed Out";
        descriptionText.text = "";
        homeButton.SetActive(false);
    }
}
