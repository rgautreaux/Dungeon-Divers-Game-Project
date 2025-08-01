using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public RaycastHit select;
    private Camera camera;

    public GameObject backButton;
    public GameObject loreButton;
    public GameObject gameInfoButton;
    public GameObject creditsButton;
    public GameObject continueButton;

    //Current Scores
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI bossCount;
    public TextMeshProUGUI monsterCount;
    public TextMeshProUGUI purchaseCount;
    public TextMeshProUGUI goldCount;
    public TextMeshProUGUI gameScoreText;

    //High Scores
    public TextMeshProUGUI lastLevel;
    public TextMeshProUGUI lastBossCount;
    public TextMeshProUGUI lastMonsterCount;
    public TextMeshProUGUI lastPurchaseCount;
    public TextMeshProUGUI lastGoldCount;
    public TextMeshProUGUI highScoreText;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit pressButton;

        // Interact with knobs and switches
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out pressButton))
        {
            if (pressButton.collider.CompareTag("Story"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    StoryButton();
                    Debug.Log("Read Story");
                }
            }
            if (pressButton.collider.CompareTag("Back"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    BackButton();
                    Debug.Log("Back to Start");
                }
            }
            if (pressButton.collider.CompareTag("Credits"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    CreditsButton();
                    Debug.Log("Check Out Game Credits");
                }
            }
            if (pressButton.collider.CompareTag("Continue"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    ContributionButton();
                    Debug.Log("Check Out Team Credits");
                }
            }
            if (pressButton.collider.CompareTag("GameInfo"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    GameInfoButton();
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
        SceneManager.LoadScene("Alien_Onslaught");
    }

    public void RestartGame()
    {
        GameStats.level = 1;
        GameStats.bossesFinished = 0;
        GameStats.monstersKilled = 0;
        GameStats.shopTrips = 0;
        GameStats.moneySpent = 0;
        GameStats.goldCoins = 0;
        GameStats.gameScore = 0;

        SceneManager.LoadScene("Alien_Onslaught");
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
        SceneManager.LoadScene("LoreScreen");
    }

    public void ContributionButton()
    {
        SceneManager.LoadScene("Contributions");
    }

    private void OnGUI()
    {
        if (levelText)
        {
            levelText.text = "Level: " + GameStats.level.ToString();
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