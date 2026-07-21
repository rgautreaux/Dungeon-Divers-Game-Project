using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public RaycastHit pressButton;
    private Camera camera;
    public static int timesPlayed;
    public AudioSource music;

    public GameObject backButton;
    public GameObject loreButton;
    public GameObject gameInfoButton;
    public GameObject creditsButton;
    public GameObject continueButton;
    public GameObject highScoreButton;


    //Current Scores
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI bossCount;
    public TextMeshProUGUI monsterCount;
    public TextMeshProUGUI purchaseCount;
    public TextMeshProUGUI goldCount;
    public TextMeshProUGUI gameScoreText;

    //High Scores
    public TextMeshProUGUI playCount;
    public TextMeshProUGUI lastLevel;
    public TextMeshProUGUI lastBossCount;
    public TextMeshProUGUI lastMonsterCount;
    public TextMeshProUGUI lastPurchaseCount;
    public TextMeshProUGUI lastGoldCount;
    public TextMeshProUGUI highScoreText;


    // Start is called before the first frame update
    void Start()
    {
        music.playOnAwake = true;
        music.loop = true;
        camera = Camera.main;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        //create ray
        Ray mouseCursor = camera.ScreenPointToRay(Input.mousePosition);
        
        // Interact with buttons
        if (Physics.Raycast(mouseCursor, out RaycastHit pressButton, 10f))
        {
            if (pressButton.collider.CompareTag("Story") || pressButton.collider.gameObject == loreButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    StoryButton();
                    Debug.Log("Read Story");
                }
            }
            if (pressButton.collider.CompareTag("Back") || pressButton.collider.gameObject == backButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    BackButton();
                    Debug.Log("Back to Start");
                }
            }
            if (pressButton.collider.CompareTag("Credits") || pressButton.collider.gameObject == creditsButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    CreditsButton();
                    Debug.Log("Check Out Game Credits");
                }
            }
            if (pressButton.collider.CompareTag("GameInfo") || pressButton.collider.gameObject == gameInfoButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    GameInfoButton();
                    Debug.Log("Learn about the Game");
                }
            }
            if (pressButton.collider.CompareTag("HighScore") || pressButton.collider.gameObject == highScoreButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    HighScoreButton();
                    Debug.Log("Learn about the Game");
                }
            }
            if (pressButton.collider.CompareTag("Continue") || pressButton.collider.gameObject == continueButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    ContinueButton();
                    Debug.Log("Learn about the Game");
                }
            }
        }
    }

    public void QuitGameButton()
    {
        Debug.Log("Quit Button");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
           Application.Quit();
#endif
    }

    public void StartGameButton()
    {
        timesPlayed += 1;
        SceneManager.LoadScene("CharaSelect");
    }

    public void RestartGame()
    {
        timesPlayed += 1;
        GameStats.level = 1;
        GameStats.bossesFinished = 0;
        GameStats.monstersKilled = 0;
        GameStats.shopTrips = 0;
        GameStats.moneySpent = 0;
        GameStats.goldCoins = 0;
        GameStats.gameScore = 0;

        SceneManager.LoadScene("Dungeon");
    }

    public void BackButton()
    {

        SceneManager.LoadScene("StartScreen");
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    public void GameInfoButton()
    {
        SceneManager.LoadScene("GameInfo");
    }

    public void StoryButton()
    {
        SceneManager.LoadScene("StoryScreen");
    }

    public void HighScoreButton()
    {
        SceneManager.LoadScene("HighScore");
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("Controls");
    }

    private void OnGUI()
    {
        if (levelText)
        {
            levelText.text = "Level: " + GameStats.level.ToString();
        }

        if (playCount)
        {
            playCount.text = "You've played Dungeon Diver " + timesPlayed.ToString() + "times";
        }

        if (bossCount)
        {
            bossCount.text = GameStats.bossesFinished.ToString() + "/8 Dragons Defeated";
        }

        if (monsterCount)
        {
            monsterCount.text = GameStats.monstersKilled.ToString() + " Total Monsters Slain";
        }
        if (goldCount)
        {
            goldCount.text = "You have earned" + GameStats.goldCoins.ToString() + "gold";
        }

        if (purchaseCount)
        {
            purchaseCount.text = "You visited [shop name] " + GameStats.shopTrips.ToString() + " times and spent " + GameStats.moneySpent.ToString() + "gold";
        }

        if (gameScoreText)
        {
            gameScoreText.text = "Game Score = " + GameStats.gameScore.ToString();
        }

        if (lastLevel)
        {
            lastLevel.text = "Highest Level: " + GameStats.highestLevel.ToString();
        }

        if (lastBossCount)
        {
            lastBossCount.text = "Most Dragons Defeated:  " + GameStats.mostBossesFinished.ToString();
        }

        if (lastMonsterCount)
        {
            lastMonsterCount.text = "Most Monsters Slain:  " + GameStats.mostMonstersKilled.ToString();
        }

        if (lastGoldCount)
        {
            lastGoldCount.text = "Most Gold Earned:  " + GameStats.mostGoldCoins.ToString() + "gold";
        }                        

        if (lastPurchaseCount)
        {
            lastPurchaseCount.text = "You visited [shop name] " + GameStats.mostShopTrips.ToString() + " times last game, and spent " + GameStats.mostMoneySpent.ToString() + "gold";
        }

        if (highScoreText)
        {
            highScoreText.text = "High Score:  " + GameStats.highScore.ToString();
        }
    }
}