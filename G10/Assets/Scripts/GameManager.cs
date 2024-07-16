using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    


    public TextMeshProUGUI timerText,scoreText,healthText,LevelDisplay;
    private float timer;
    public Image healthbar;

    [SerializeField] public TextAsset fourLetters, fiveLetters, sixLetters, sevenLetters;
    [SerializeField] public List<WordObj> wordsObjects = new List<WordObj>();
    [SerializeField] public List<GameObject> letterPrefabs = new List<GameObject>();
    [SerializeField] public Sprite blueTile, redTile, greenTile, yellowTile, grayTile;
    [SerializeField] public List<GameObject> buttons = new List<GameObject>();
    public GameObject enterButton, deleteButton;
    
    private List<string> generatedWords = new List<string>();

    [Header("Debug Variables")]
    [SerializeField] public string currentWordtoGuess = "";
    [SerializeField] public int currentWordLength = 0;
    [SerializeField] public int currentWordIndex = 0;
    [SerializeField] public string typedWord = "";
    [SerializeField] public string lastGuess = "";
    [SerializeField] public int attempts = 5; //0 out of 5
    [SerializeField] public int TotalScore,CurrentScore,Guesses,HoleInOne,LettersPlayed;


    private List<string> wordStaringPoint = new List<string>() { "", "", "", "", "", "", "", "", "", ""};

    private bool chlMode;
    private bool canPlay = true;
    public bool canInterract = true;
    public bool usingHint = false;
   
    private char hintLetter;

    private int pointsForGreen = 100;
    private int pointsForYellow = 50;
    private int pointsForRed = 5;
    private int pointsForHoleInOne = 1000;
    private int pointsForGuessed = 150;

    private float currentMultiplier = 1;
    private float maxMultiplier = 2.5f;
    private float multiplierGain = .25f;
    private int BoardCleared;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BoardCleared = 0;
        
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            chlMode = false;
            GS_CanvasManager.instance.HintsGO.transform.DOScale(0, 0f);
            GS_CanvasManager.instance.KeysGO.transform.DOScale(0, 0f);
            StartCoroutine(Timer());
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("ChallengeScene"))
        {
            chlMode = true;
            GS_CanvasManager.instance.HintsGO.transform.DOScale(0, 0f);
            StartCoroutine(Timer());
        }
        TotalScore = 0;
        CurrentScore = 0;
        LettersPlayed = 0;
        string newWord = "";
        int currentWordIndex = 0;
        LevelDisplay.text = "Level: " + (currentWordIndex + 1);
        string[] lines = fourLetters.text.Split('\n'); //Generate 3 Four letter words
        for (int i = 0; i < 3; i++) 
        {
            newWord = lines[Random.Range(0, lines.Length)];

            generatedWords.Add(newWord);
            //Debug.Log(newWord);
            wordsObjects[currentWordIndex].word = newWord;
            wordsObjects[currentWordIndex].Init();
            currentWordIndex++;
        }
        lines = fiveLetters.text.Split('\n');   //Generate 3 Five letter words
        for (int i = 0; i < 3; i++)
        {
            newWord = lines[Random.Range(0, lines.Length)];
            generatedWords.Add(newWord);
            //Debug.Log(newWord);
            wordsObjects[currentWordIndex].word = newWord;
            wordsObjects[currentWordIndex].Init();
            currentWordIndex++;
        }
        lines = sixLetters.text.Split('\n');   //Generate 3 Six letter words
        for (int i = 0; i < 3; i++)
        {
            newWord = lines[Random.Range(0, lines.Length)];
            generatedWords.Add(newWord);
            //Debug.Log(newWord);
            wordsObjects[currentWordIndex].word = newWord;
            wordsObjects[currentWordIndex].Init();
            currentWordIndex++;
        }
        lines = sevenLetters.text.Split('\n'); //Generate 1 Seven letter word
        newWord = lines[Random.Range(0, lines.Length)];
        generatedWords.Add(newWord);
        //Debug.Log(newWord);
        wordsObjects[currentWordIndex].word = newWord;
        wordsObjects[currentWordIndex].Init();

        currentWordtoGuess = generatedWords[0];
        currentWordLength = currentWordtoGuess.Length;
        if(chlMode == false)
            GenerateHint();

        
        GS_CanvasManager.instance.HintButtonsAnimation(chlMode);
    }

    private IEnumerator Timer()
    {
        
        while (canPlay)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer % 60F);
            int milliseconds = Mathf.FloorToInt((timer * 100F) % 100F);
            yield return timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");       
        }

        yield return null;
    }
      
    public void AddLetter(char letter)  //Letter Clicked and placed 
    {
        if (!canInterract) return;
        if(usingHint)
        {
            hintLetter = letter;
        }
        else if (!WordsMatch(typedWord, currentWordtoGuess))
        {
            if (typedWord.Contains(" "))
            {
                for (int i = 0; i < typedWord.Length; i++)
                {
                    if (typedWord[i] == ' ')
                    {
                        StringBuilder sb = new StringBuilder(typedWord);
                        sb[i] = letter;
                        typedWord = sb.ToString();
                        GameObject instantiatedLetter = Instantiate(letterPrefabs.Find(x => x.name[0].ToString().ToLower() == letter.ToString()));
                        instantiatedLetter.transform.SetParent(wordsObjects[currentWordIndex].slots[i].gameObject.transform);
                        instantiatedLetter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                        instantiatedLetter.transform.localScale = new Vector3(1, 1, 1);
                        RectTransform RT = instantiatedLetter.GetComponent<RectTransform>();
                        RT.localPosition = Vector3.up *6;
                        instantiatedLetter.transform.GetComponent<Image>().sprite = StoreManager.instance.keyboardColors[StoreManager.instance.currentlyUsedKeyboardIndex];
                        if (!typedWord.Contains(" ") && typedWord.Length == currentWordLength - 1)
                            GS_CanvasManager.instance.EnterHighlight(1);
                            GS_CanvasManager.instance.BackspaceHighlight(1);
                        instantiatedLetter.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.001f);
                        instantiatedLetter.transform.DOScale(new Vector3(1, 1, 1), .7f);
                        instantiatedLetter.transform.rotation = new Quaternion(0, 0, 0, 0);
                        return;
                    }
                }
            }
            else
            {
                if (typedWord.Length < currentWordLength - 1)
                {
                    typedWord += letter;
                    GameObject instantiatedLetter = Instantiate(letterPrefabs.Find(x => x.name[0].ToString().ToLower() == letter.ToString()));
                    instantiatedLetter.transform.SetParent(wordsObjects[currentWordIndex].slots[typedWord.Length - 1].gameObject.transform);
                    instantiatedLetter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                    instantiatedLetter.transform.localScale = new Vector3(1, 1, 1);
                    RectTransform RT = instantiatedLetter.GetComponent<RectTransform>();
                    RT.localPosition = Vector3.up * 6;
                    instantiatedLetter.transform.GetComponent<Image>().sprite = StoreManager.instance.keyboardColors[StoreManager.instance.currentlyUsedKeyboardIndex];
                    if (!typedWord.Contains(" ") && typedWord.Length == currentWordLength - 1)
                        GS_CanvasManager.instance.EnterHighlight(1);
                        GS_CanvasManager.instance.BackspaceHighlight(1);

                    instantiatedLetter.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.001f);
                    instantiatedLetter.transform.DOScale(new Vector3(1, 1, 1), .7f);
                    instantiatedLetter.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }
    }

    public void RemoveLetter()    //Remove last letter
    {
        GS_CanvasManager.instance.EnterHighlight(0);

        if (!canInterract) return;
        if (typedWord.Length > 0)
        {
            bool deleted = false;
            int currentIndex = typedWord.Length - 1;
            for (int i = currentIndex; i >= 0; i--)
            {
                if (deleted) return;

                try
                {
                    if (wordsObjects[currentWordIndex].slots[currentIndex].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite.name != greenTile.name)
                    {
                        if (typedWord[currentIndex] != ' ')
                        {
                            Destroy(wordsObjects[currentWordIndex].slots[currentIndex].transform.GetChild(0).gameObject);

                            StringBuilder sb = new StringBuilder(typedWord);
                            sb[currentIndex] = ' ';
                            typedWord = sb.ToString();
                            deleted = true;
                        }
                    }
                    else
                    {
                        if (currentIndex > 0)
                        {
                            currentIndex--;
                        }
                        else
                        {
                            deleted = true;
                        }
                    }
                }
                catch
                {
                    if (currentIndex > 0)
                    {
                        currentIndex--;
                    }
                    else
                    {
                        deleted = true;
                    }
                }
            }          
        }
    }

    public void EnterPressed()  // Confirm word guessed and verify
    {
        if (!canInterract) return;
        if (typedWord.Length == currentWordLength - 1 && typedWord.Contains(" ") == false)
        {
            if (attempts == 0)
            {
                if (chlMode == true)
                {
                    //Debug.Log("You lost :(");
                    GS_CanvasManager.instance.GameOverCHALLENGEDialog();
                    GS_CanvasManager.instance.UpdateChallengeTime();
                    canPlay = false;
                    if (StoreManager.instance.adfree == true)
                        Debug.Log("Ad were removed");
                    else
                        AdmobManager.instance.GameOverAd();
                }
                else
                {
                    {
                        //Debug.Log("You lost :(");
                        GS_CanvasManager.instance.GameOverDialog();
                        canPlay = false;
                        if (StoreManager.instance.adfree == true)
                            Debug.Log("Ad were removed");
                        else
                            AdmobManager.instance.GameOverAd();
                    }
                }
            }
            else
            {
                StartCoroutine(WordCheckAnimation());
                GS_CanvasManager.instance.EnterHighlight(0);
            }
        }
    }

    private bool WordsMatch(string word1, string word2)
    {
        bool match = true;
        if (word1.Length == 0 || word1.Length < currentWordLength - 1) return false;
        for (int i = 0; i < word2.Length; i++)
        {
            try { if (word2[i] != word1[i]) return false; } catch { }
        }
        return match;
    }

    private IEnumerator WordCheckAnimation()
    {
        if (typedWord.Equals(lastGuess))
        {
            lastGuess = typedWord;
            yield break;
        }
        else
            lastGuess = typedWord;


        Guesses += 1;
        canInterract = false;
        if (currentWordIndex != 9)
        {
            LettersPlayed += typedWord.Length - 1;
        }
        else
            LettersPlayed += typedWord.Length;

        bool matched = WordsMatch(typedWord, currentWordtoGuess);

        int numberOfGreenTiles = 0;

        for (int i = 0; i < typedWord.Length; i++)
        {
            if (wordsObjects[currentWordIndex].slots[i].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite.name == greenTile.name) //Letter and position correct
            {
                numberOfGreenTiles++;
            }
        }

        if (!matched && currentWordIndex <= 2 && numberOfGreenTiles >= 2)
        {
            matched = false; //WordsMatch(typedWord, currentWordtoGuess)
            int yellowsOnScreen = 0;
            foreach (var item in buttons)
            {
                if (item.GetComponent<Image>().sprite.name == yellowTile.name)
                    yellowsOnScreen++;    
            }
            string[] words = fourLetters.text.Split('\n');
            foreach (var word in words)
            {
                if (!matched && attempts < 3 && yellowsOnScreen == 0)
                {
                    matched = WordsMatch(typedWord, word);
                    if (matched)
                    {
                        currentWordtoGuess = word;
                    }
                }
            }
        }

        for (int i = 0; i < typedWord.Length; i++)
        {
            if (typedWord[i] == currentWordtoGuess[i]) //Letter and position correct
            {
                if (wordsObjects[currentWordIndex].slots[i].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite.name != greenTile.name) CurrentScore += pointsForGreen;
                wordsObjects[currentWordIndex].slots[i].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = greenTile;
                GameObject letterButton = buttons.Find(x => x.name[0].ToString().ToLower() == typedWord[i].ToString());
                letterButton.GetComponent<Image>().sprite = greenTile;
                
            }
            else if(CheckForYellow(i, currentWordtoGuess, typedWord.ToCharArray()))
            {
                if(wordsObjects[currentWordIndex].slots[i].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite.name != yellowTile.name) CurrentScore += pointsForYellow;
                wordsObjects[currentWordIndex].slots[i].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = yellowTile;
                GameObject letterButton = buttons.Find(x => x.name[0].ToString().ToLower() == typedWord[i].ToString());
                letterButton.GetComponent<Image>().sprite = yellowTile;
            }
            else
            {
                if (typedWord[i] != ' ')
                {
                    wordsObjects[currentWordIndex].slots[i].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = redTile;
                    GameObject letterButton = buttons.Find(x => x.name[0].ToString().ToLower() == typedWord[i].ToString());
                    if (letterButton.GetComponent<Image>().sprite.name != yellowTile.name && !currentWordtoGuess.ToLower().Contains(typedWord[i].ToString()))
                    {
                        letterButton.GetComponent<Image>().sprite = grayTile;
                        letterButton.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    CurrentScore += 5;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }

        if (matched) //Word matches move to next Guess frame
        {
            StartCoroutine(WordJump(currentWordLength, currentWordIndex));

            if (chlMode)
            {
                CurrentScore += pointsForGuessed;
                if (attempts == 5) { CurrentScore += pointsForHoleInOne; }
                CurrentScore *= Mathf.CeilToInt(attempts);
                TotalScore += CurrentScore;             //Add score
                scoreText.text = TotalScore.ToString();
                currentWordIndex++;
                if(currentWordIndex==10)
                    LevelDisplay.text = "Level: " + ((currentWordIndex));
                else
                    LevelDisplay.text = "Level: " + ((currentWordIndex + 1));
            }
            else 
            {
                CurrentScore += pointsForGuessed;
                if (attempts == 5) { CurrentScore += pointsForHoleInOne; }
                CurrentScore *= Mathf.CeilToInt(currentMultiplier);
                TotalScore += CurrentScore;             //Add score
                scoreText.text = TotalScore.ToString();
                currentWordIndex++;
                LevelDisplay.text = "Level: " + ((currentWordIndex + 1) + (BoardCleared * 10));
            }
            MusicTransition.instance.OnRightGuess(); //Play Effect

            GS_CanvasManager.instance.MoveBorder(currentWordIndex);
            if(currentWordIndex == 6)
            {
                MusicTransition.instance.TriggerMusicChange();
            }
            if(attempts == 5)
            {
                HintController.instance.AddHint();
            }

            if(currentWordIndex == 10)
            {
                MusicTransition.instance.OnWin();
                yield return new WaitForSeconds(1.5f);

                if (chlMode == true)
                {
                    EndGameStats.instance.ChallengeWinStats(TotalScore, Guesses, PlayerPrefs.GetInt("ChallengesWon") + 1, HoleInOne, timerText.text, Mathf.CeilToInt((TotalScore * (HoleInOne + 1)) / 10)); //Send End Game Scores & Stats
                    GS_CanvasManager.instance.ChallengeWinDialog();
                    LevelDisplay.text = "Challenge Cleared!";
                    Achievements.instance.StepGamePlayAchievements(LettersPlayed,Guesses,HoleInOne);
                    Achievements.instance.StepChallengerAchievements();
                    //Add score to Leaderboard
                    CloudSaveManager.instance.PostChallengeScore(TotalScore);
                    GS_CanvasManager.instance.UpdateChallengeTime();
                    StoreManager.instance.UpdateKeys(1);
                    yield break;

                }
                else
                {
                    BoardCleared += 1;
                    Achievements.instance.StepGamePlayAchievements(LettersPlayed, Guesses, HoleInOne);
                    Achievements.instance.GrantExpertAchievements(currentWordIndex * BoardCleared);
                    
                    currentWordIndex = 0;
                    Level10Reset();
                    for (int i = 0; i <= 9; i++)
                    {

                        for (int y = 0; y < wordsObjects[i].slots.Count; y++)
                        {
                            Destroy(wordsObjects[i].slots[y].transform.GetChild(0).gameObject);
                        }

                    }
                    //Here add code for the win menu.

                    currentMultiplier += multiplierGain;

                    if (currentMultiplier > maxMultiplier) currentMultiplier = maxMultiplier;
                }



                yield return null;
            }

            currentWordtoGuess = generatedWords[currentWordIndex];
            currentWordLength = currentWordtoGuess.Length;
            typedWord = wordStaringPoint[currentWordIndex];
            healthbar.fillAmount = 1;
            healthText.text = "5";
            foreach (var item in buttons)
            {
                item.GetComponent<Image>().sprite = StoreManager.instance.keyboardColors[StoreManager.instance.currentlyUsedKeyboardIndex];
                item.GetComponent<BoxCollider2D>().enabled = true;
                yield return new WaitForSeconds(0.02f);
            }

            if(currentWordIndex != 9 && chlMode == false)
            {
                GenerateHint();
            }

            if (attempts == 5)
            {
                MusicTransition.instance.PlayHoleInOne();
                HoleInOne += 1;
            }
            attempts = 5;        
        }
        else
        {
            if (chlMode)
            {
                TotalScore += Mathf.CeilToInt(CurrentScore * attempts);                 //Add score
                scoreText.text = TotalScore.ToString();
            }
            else
            {
                TotalScore += CurrentScore;                 //Add score
                scoreText.text = TotalScore.ToString();
            }
            attempts--;
            MusicTransition.instance.OnWrongGuess();    //Play Effect

            healthbar.fillAmount = attempts * .20f;
            healthText.text = attempts.ToString();
            if (attempts == 0)
            {
                MusicTransition.instance.OnGameOver();  //Play Effect

                if (chlMode == true)
                {
                    canPlay = false;
                    Achievements.instance.StepGamePlayAchievements(LettersPlayed, Guesses, HoleInOne);
                    EndGameStats.instance.GameOverStats(chlMode,TotalScore, Guesses, (currentWordIndex), PlayerPrefs.GetInt("ChallengesWon"), HoleInOne, timerText.text, Mathf.CeilToInt((TotalScore*(HoleInOne+1))/10)); //Send End Game Scores & Stats
                    GS_Stats_Handler.instance.AddtoStats(Guesses, LettersPlayed, HoleInOne, 0, 1, (currentWordIndex), 0, TotalScore);
                    //Debug.Log("You lost :(");
                    GS_CanvasManager.instance.GameOverCHALLENGEDialog();
                    GS_CanvasManager.instance.UpdateChallengeTime();
                    if (StoreManager.instance.adfree == true)
                        Debug.Log("Ad were removed");
                    else
                        AdmobManager.instance.GameOverAd();

                }
                else
                {
                    Achievements.instance.StepGamePlayAchievements(LettersPlayed, Guesses, HoleInOne);
                    EndGameStats.instance.GameOverStats(chlMode, TotalScore, Guesses, (currentWordIndex + (BoardCleared *10)), PlayerPrefs.GetInt("ChallengesWon"), HoleInOne, timerText.text, Mathf.CeilToInt((TotalScore * (HoleInOne + 1)) / 10)); //Send End Game Scores & Stats
                    GS_CanvasManager.instance.GameOverDialog();
                    GS_Stats_Handler.instance.AddtoStats(Guesses, LettersPlayed, HoleInOne, 0, 1, (currentWordIndex), TotalScore, 0);
                    CloudSaveManager.instance.PostNormalHighestLevel(currentWordIndex + (BoardCleared * 10));

                    //Debug.Log("You lost :(");
                    canPlay = false;
                    if (StoreManager.instance.adfree == true)
                        Debug.Log("Ad were removed");
                    else
                        AdmobManager.instance.GameOverAd();

                }
            }
        }
        CurrentScore = 0;
        canInterract = true;
        yield return null;
    }

    private IEnumerator WordJump(int l, int j)
    {
        for (int i = 0; i < l - 1; i++)
        {
            wordsObjects[j].slots[i].gameObject.transform.GetChild(0).gameObject.transform.DOLocalJump(new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), 50, 1, 1f);
            //RectTransform RT = wordsObjects[j].slots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            //RT.localPosition = Vector3.up * 6;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    private void GenerateHint()
    {
        bool revealed = false;

        bool allGreen = true;
        for (int i = 0; i < currentWordLength - 1; i++)
        {
            try
            {
                if (wordsObjects[currentWordIndex].slots[i].gameObject.transform.GetChild(0)?.GetComponentInChildren<Image>().sprite.name != greenTile.name) allGreen = false;
            }
            catch
            {
                allGreen = false;
            }
        }

        if (allGreen) return;

        while (!revealed)
        {
            int randomLetter = Random.Range(0, currentWordLength - 1);
            bool tileExists = wordsObjects[currentWordIndex].slots[randomLetter].gameObject.transform.childCount == 0 ? false : true;
            if(!tileExists)
            {
                revealed = true;
                if(string.IsNullOrEmpty(typedWord)) typedWord = new string(' ', currentWordLength - 1);
                StringBuilder sb = new StringBuilder(typedWord);
                sb[randomLetter] = currentWordtoGuess[randomLetter];
                typedWord = sb.ToString();
                GameObject instantiatedLetter = Instantiate(letterPrefabs.Find(x => x.name[0].ToString().ToLower() == currentWordtoGuess[randomLetter].ToString()));
                instantiatedLetter.transform.SetParent(wordsObjects[currentWordIndex].slots[randomLetter].gameObject.transform);
                instantiatedLetter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                instantiatedLetter.transform.localScale = new Vector3(1, 1, 1);
                RectTransform RT = instantiatedLetter.GetComponent<RectTransform>();
                RT.localPosition = Vector3.up * 6;
                wordsObjects[currentWordIndex].slots[randomLetter].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = greenTile;
                instantiatedLetter.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.001f);
                instantiatedLetter.transform.DOScale(new Vector3(1, 1, 1), .7f);
                instantiatedLetter.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (wordsObjects[currentWordIndex].slots[randomLetter].gameObject.transform.GetChild(0)?.GetComponentInChildren<Image>().sprite.name != greenTile.name)
            {
                revealed = true;
                if (string.IsNullOrEmpty(typedWord)) typedWord = new string(' ', currentWordLength - 1);
                StringBuilder sb = new StringBuilder(typedWord);
                sb[randomLetter] = currentWordtoGuess[randomLetter];
                typedWord = sb.ToString();
                GameObject instantiatedLetter = Instantiate(letterPrefabs.Find(x => x.name[0].ToString().ToLower() == currentWordtoGuess[randomLetter].ToString()));
                instantiatedLetter.transform.SetParent(wordsObjects[currentWordIndex].slots[randomLetter].gameObject.transform);
                instantiatedLetter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                instantiatedLetter.transform.localScale = new Vector3(1, 1, 1);
                RectTransform RT = instantiatedLetter.GetComponent<RectTransform>();
                RT.localPosition = Vector3.up * 6;
                wordsObjects[currentWordIndex].slots[randomLetter].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = greenTile;
                instantiatedLetter.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.001f);
                instantiatedLetter.transform.DOScale(new Vector3(1, 1, 1), .7f);
                instantiatedLetter.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
    }

    public void RevealLetter(int i, int wordIndex)
    {
        bool tileExists = wordsObjects[wordIndex].slots[i].gameObject.transform.childCount == 0 ? false : true;
        if (!tileExists)
        { 
            if (wordIndex == currentWordIndex)
            {
                if (string.IsNullOrEmpty(typedWord)) typedWord = new string(' ', generatedWords[wordIndex].Length - 1);
                StringBuilder sb = new StringBuilder(typedWord);
                sb[i] = generatedWords[wordIndex][i];
                typedWord = sb.ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(wordStaringPoint[wordIndex])) wordStaringPoint[wordIndex] = new string(' ', generatedWords[wordIndex].Length - 1);
                StringBuilder sb = new StringBuilder(wordStaringPoint[wordIndex]);
                sb[i] = generatedWords[wordIndex][i];
                wordStaringPoint[wordIndex] = sb.ToString();
            }
            
            GameObject instantiatedLetter = Instantiate(letterPrefabs.Find(x => x.name[0].ToString().ToLower() == generatedWords[wordIndex][i].ToString()));
            instantiatedLetter.transform.SetParent(wordsObjects[wordIndex].slots[i].gameObject.transform);
            instantiatedLetter.transform.rotation = Quaternion.identity;
            instantiatedLetter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            instantiatedLetter.transform.localScale = new Vector3(1, 1, 1);
            RectTransform RT = instantiatedLetter.GetComponent<RectTransform>();
            RT.localPosition = Vector3.up * 6;
            wordsObjects[wordIndex].slots[i].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = greenTile;
            instantiatedLetter.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.001f);
            instantiatedLetter.transform.DOScale(new Vector3(1, 1, 1), .7f);
            instantiatedLetter.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (wordsObjects[wordIndex].slots[i].gameObject.transform.GetChild(0)?.GetComponentInChildren<Image>().sprite.name != greenTile.name)
        {
            if (wordIndex == currentWordIndex)
            {
                if (string.IsNullOrEmpty(typedWord)) typedWord = new string(' ', generatedWords[wordIndex].Length - 1);
                StringBuilder sb = new StringBuilder(typedWord);
                sb[i] = generatedWords[wordIndex][i];
                typedWord = sb.ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(wordStaringPoint[wordIndex])) wordStaringPoint[wordIndex] = new string(' ', generatedWords[wordIndex].Length - 1);
                StringBuilder sb = new StringBuilder(wordStaringPoint[wordIndex]);
                sb[i] = generatedWords[wordIndex][i];
                wordStaringPoint[wordIndex] = sb.ToString();
            }
            Destroy(wordsObjects[wordIndex].slots[i].gameObject.transform.GetChild(0).gameObject);
            GameObject instantiatedLetter = Instantiate(letterPrefabs.Find(x => x.name[0].ToString().ToLower() == generatedWords[wordIndex][i].ToString()));
            instantiatedLetter.transform.SetParent(wordsObjects[wordIndex].slots[i].gameObject.transform);
            instantiatedLetter.transform.rotation = Quaternion.identity;
            instantiatedLetter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            instantiatedLetter.transform.localScale = new Vector3(1, 1, 1);
            instantiatedLetter.transform.GetComponentInChildren<Image>().sprite = greenTile;
            RectTransform RT = instantiatedLetter.GetComponent<RectTransform>();
            RT.localPosition = Vector3.up * 6;
            instantiatedLetter.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.001f);
            instantiatedLetter.transform.DOScale(new Vector3(1, 1, 1), .7f);
            instantiatedLetter.transform.rotation = new Quaternion(0, 0, 0, 0);
            //wordsObjects[wordIndex].slots[i].gameObject.transform.GetChild(1).GetComponentInChildren<Image>().sprite = greenTile;
        }

        usingHint = false;
    }

    private bool CheckForYellow(int index, string word, char[] letters)
    {
        int letterCount = 0;
        int incorrectCountBeforeIndex = 0;
        int correctCount = 0;
        for (int i = 0; i < word.Length - 1; i++)
        {
            if (word[i] == letters[index])
            {
                letterCount++;
            }
            if (letters[i] == letters[index] && word[i] == letters[index])
            {
                correctCount++;
            }
            if (i < index && letters[i] == letters[index] && word[i] != letters[index])
            {
                incorrectCountBeforeIndex++;
            }
        }
        return letterCount - correctCount - incorrectCountBeforeIndex > 0;
    }


    public void Level10Reset()
    {
        generatedWords.Clear();
        //StartCoroutine(LetterFallAnimation()); //Clear current board animation


        string newWord = "";
        int currentWordIndex = 0;
        string[] lines = fourLetters.text.Split('\n'); //Generate 3 Four letter words
        for (int i = 0; i < 3; i++)
        {
            newWord = lines[Random.Range(0, lines.Length)];
            generatedWords.Add(newWord);
            //Debug.Log(newWord);
            wordsObjects[currentWordIndex].word = newWord;
            wordsObjects[currentWordIndex].Init();
            currentWordIndex++;
        }
        lines = fiveLetters.text.Split('\n');   //Generate 3 Five letter words
        for (int i = 0; i < 3; i++)
        {
            newWord = lines[Random.Range(0, lines.Length)];
            generatedWords.Add(newWord);
            //Debug.Log(newWord);
            wordsObjects[currentWordIndex].word = newWord;
            wordsObjects[currentWordIndex].Init();
            currentWordIndex++;
        }
        lines = sixLetters.text.Split('\n');   //Generate 3 Six letter words
        for (int i = 0; i < 3; i++)
        {
            newWord = lines[Random.Range(0, lines.Length)];
            generatedWords.Add(newWord);
            //Debug.Log(newWord);
            wordsObjects[currentWordIndex].word = newWord;
            wordsObjects[currentWordIndex].Init();
            currentWordIndex++;
        }
        lines = sevenLetters.text.Split('\n'); //Generate 1 Seven letter word
        newWord = lines[Random.Range(0, lines.Length)];
        generatedWords.Add(newWord);
        //Debug.Log(newWord);
        wordsObjects[currentWordIndex].word = newWord;
        wordsObjects[currentWordIndex].Init();

        currentWordtoGuess = generatedWords[0];
        currentWordLength = currentWordtoGuess.Length;
        GS_CanvasManager.instance.MoveBorder(0);


    }

    IEnumerator LetterFallAnimation()   //Drop Title on play button
    {

        for (int i = 0; i <= 9; i++)
        {

            for (int y = 0; y < wordsObjects[i].slots.Count; y++)
            {
                wordsObjects[i].slots[y].transform.GetChild(0).gameObject.transform.DOShakeScale(0f, 2f).SetEase(Ease.OutBounce);
            }

        }
        yield return new WaitForSeconds(3);

    }

}
