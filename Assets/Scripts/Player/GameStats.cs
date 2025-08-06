using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//using UnityEditor.Timeline.TimelinePlaybackControls;


public class GameStats : MonoBehaviour
{
    public GameObject player;
    public new Camera camera;
    public float maxDistance = 3f;

    //Current Scores
    public static int level = 1;
    public static int bossesFinished = 0;
    public static int monstersKilled = 0;
    public static int shopTrips = 0;
    public static int moneySpent = 0;
    public static int goldCoins = 0;
    public static int gameScore = 0;
    public static int health;
    public static int maxHealth;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI bossCount;
    public TextMeshProUGUI monsterCount;
    public TextMeshProUGUI purchaseCount;
    public TextMeshProUGUI goldCount;
    public TextMeshProUGUI gameScoreText;

    //High Scores
    public static int totalTimesPlayed;
    public static int highestLevel = 0;
    public static int mostBossesFinished = 0;
    public static int mostMonstersKilled = 0;
    public static int mostShopTrips = 0;
    public static int mostMoneySpent = 0;
    public static int mostGoldCoins = 0;
    public static int highScore = 0;

    public TextMeshProUGUI playCount;
    public TextMeshProUGUI lastLevel;
    public TextMeshProUGUI lastBossCount;
    public TextMeshProUGUI lastMonsterCount;
    public TextMeshProUGUI lastPurchaseCount;
    public TextMeshProUGUI lastGoldCount;
    public TextMeshProUGUI highScoreText;

    public bool FirstBoss = false;
    public bool SecBoss = false;
    public bool ThirdBoss = false;
    public bool FourthBoss = false;
    public bool Final = false;

    // Start is called before the first frame update
    void Start()
    { 
        camera = Camera.main;

        totalTimesPlayed = MainMenu.timesPlayed;

        float health = ProtagMovement.health;
        float maxHealth = ProtagMovement.maxHealth;

        playCount.text = "You've played Dungeon Diver " + totalTimesPlayed.ToString() + " times";
        levelText.text = "Level " + level.ToString();
        bossCount.text = bossesFinished.ToString() + "/8 Dragons Defeated";
        monsterCount.text = monstersKilled.ToString() + " Total Monsters Slain";
        goldCount.text = "You have earned" + goldCoins.ToString() + "gold";
        purchaseCount.text = "You visited Undead Deals " + shopTrips.ToString() + " times and spent " + moneySpent.ToString() + "gold";
        gameScoreText.text = "Game Score = " + gameScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevel(15, 25, 5, 5, 5, 0.5F);
        UpdateHighScore();

        float health = ProtagMovement.health;
        float maxHealth = ProtagMovement.maxHealth;

        if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject.name == "BossDoor0" || hit.collider.CompareTag("Boss1"))
            {
                FirstBoss = true;
                gameScore += 20;
                UpdateHealth(50, health, maxHealth);
                UpdateLevel(25, 50, 10, 10, 10, 1);
                SceneManager.LoadScene("Boss1");


            }
            else if (hit.collider.gameObject.name == "BossDoor1" || hit.collider.CompareTag("Boss2"))
            {
                SecBoss = true;
                gameScore += 30;
                UpdateHealth(50, health, maxHealth);
                UpdateLevel(50, 50, 20, 20, 20, 1);
                SceneManager.LoadScene("Boss2");

            }
            else if (hit.collider.gameObject.name == "BossDoor2" || hit.collider.CompareTag("Boss3"))
            {
                ThirdBoss = true;
                gameScore += 40;
                UpdateHealth(50, health, maxHealth);
                UpdateLevel(75, 50, 30, 30, 30, 1);
                SceneManager.LoadScene("Boss3");

            }
            else if (hit.collider.gameObject.name == "BossDoor3" || hit.collider.CompareTag("Boss4"))
            {
                FourthBoss = true;
                gameScore += 50;
                UpdateHealth(50, health, maxHealth);
                UpdateLevel(100, 50, 40, 40, 40, 1);
                SceneManager.LoadScene("Boss4");


            }
            else if (hit.collider.gameObject.name == "FinalBoss" || hit.collider.CompareTag("Finale"))
            {
                if (FirstBoss && SecBoss && ThirdBoss && FourthBoss == true)
                {
                    Final = true;
                    gameScore += 100;
                    UpdateHealth(100, health, maxHealth);
                    UpdateLevel(25, 50, 50, 50, 50, 1);
                    SceneManager.LoadScene("FinalBoss");
                }
                else
                {
                    Debug.Log("You're not ready yet.");
                }

            }
            else if (hit.collider.gameObject.name == "SkeleShop")
            {
                Debug.Log("Enter the Shop");
                shopTrips += 1;
                SceneManager.LoadScene("Shop");

            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Instant Win Cheat");
            bossesFinished = 5;
            gameScore += 60;
        }
    }

    private void OnGUI()
    {
        levelText.text = "Level " + level.ToString();
        bossCount.text = bossesFinished.ToString() + "/8 Dragons Defeated";
        monsterCount.text = monstersKilled.ToString() + " Total Monsters Slain";
        goldCount.text = "You have earned" + goldCoins.ToString() + "gold";
        purchaseCount.text = "You visited Undead Deals " + shopTrips.ToString() + " times and spent " + moneySpent.ToString() + "gold";
        gameScoreText.text = "Game Score = " + gameScore.ToString();
    

        if (bossesFinished >= 8)
        {
            EndGame();
        }
    }

    public static void UpdateMonstersKilled()
    {
        monstersKilled += 1;
        goldCoins += 10;
        gameScore += 15;
    }

    public static void UpdateDragonsKilled()
    {
        bossesFinished += 1;
        goldCoins += 50;
        gameScore += 30;
    }

    public static void UpdateCurrency(float money)
    {
        goldCoins += 1;
        gameScore += 10;
    }

    public static void UpdateHealth(float healthIncrease, float currenthealth, float maxHealth)
    {
        ProtagMovement.health += healthIncrease;
        if (currenthealth > maxHealth) currenthealth = maxHealth;
        gameScore += 5;
    }
    public static void UpdateLevel(float levelThreshold, int healthMaxIncrease, float attackUpgrade, float magicUpgrade, int staminaIncrease, float armorUpgrade)
    {
        if (gameScore / levelThreshold == 0)
        {
            level += 1;
            ProtagMovement.maxHealth += healthMaxIncrease;
            UpdateAttack(attackUpgrade);
            UpdateMagic(magicUpgrade);
            UpdateStaina(staminaIncrease);
            UpdateArmor(armorUpgrade);
            levelThreshold *= 5;

            Shopping.addAttack *= 2;
            Shopping.addArmor *= 2;
            Shopping.addMagic *= 2;

            Shopping.attackPrice += 5;
            Shopping.armorPrice += 5;
            Shopping.magicPrice += 5;
            Shopping.potionPrice += 5;

            Shopping.itemLimit += Random.Range(1, 5);
        }
    }

    public static void UpdateAttack(float attackUpgrade)
    {
        ProtagMovement.attackPower += attackUpgrade;
        gameScore += 5;
    }

    public static void UpdateArmor(float armorUpgrade)
    {
        ProtagMovement.armorPower += armorUpgrade;
        gameScore += 5;
    }

    public static void UpdateMagic(float magicUpgrade)
    {
        ProtagMovement.magicPower += magicUpgrade;
        gameScore += 5;
    }

    public static void UpdateStaina(float staminaIncrease)
    {
        ProtagMovement.maxStamina += staminaIncrease;
        gameScore += 5;
    }

    public static void UpdateHighScore()
    {
        if (level > highestLevel)
        {
            highestLevel = level;
        }
        if (bossesFinished > mostBossesFinished)
        {
            mostBossesFinished = bossesFinished;
        }
        if (monstersKilled > mostMonstersKilled)
        {
            mostMonstersKilled = monstersKilled;
        }
        if (shopTrips > mostShopTrips)
        {
            mostShopTrips = shopTrips;
        }
        if (moneySpent > mostMoneySpent)
        {
            mostMoneySpent = moneySpent;
        }
        if(goldCoins > mostGoldCoins)
        {
            mostGoldCoins = goldCoins; 
        }
        if(gameScore > highScore)
        {
            highScore = gameScore;
        }
}

    void EndGame()
    {
        SceneManager.LoadScene("WinScreen");
    }

}

