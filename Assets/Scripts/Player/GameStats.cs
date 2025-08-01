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

    public bool FirstBoss = false;
    public bool SecBoss = false;
    public bool ThirdBoss = false;
    public bool FourthBoss = false;
    public bool Final = false;

    // Start is called before the first frame update
    void Start()
    {
        float health = player.GetComponent<ProtagMovement>().health;
        float maxHealth = player.GetComponent<ProtagMovement>().maxHealth;

        bossCount.text = bossesFinished.ToString() + "/5 Boss Fights Defeated";
        monsterCount.text = monstersKilled.ToString() + " Total Monsters Slain";
        goldCount.text = "You have earned" + goldCoins.ToString() + "gold";
        purchaseCount.text = "You visited [shop name] " + shopTrips.ToString() + " times and spent " + moneySpent.ToString() + "gold";
        gameScoreText.text = "Game Score = " + gameScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevel(player, 15, 25, 5, 5);

        float health = player.GetComponent<ProtagMovement>().health;
        float maxHealth = player.GetComponent<ProtagMovement>().maxHealth;

        if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject.name  == "Boss1")
            {
                FirstBoss = true;
                bossesFinished += 1;
                gameScore += 20;
                UpdateHealth(player, 50, health, maxHealth);
                UpdateLevel(player, 25, 50, 10, 10);

            }
            else if (hit.collider.gameObject.name == "Boss2")
            {
                SecBoss = true;
                bossesFinished += 1;
                gameScore += 30;
                UpdateHealth(player, 50, health, maxHealth);
                UpdateLevel(player, 50, 50, 20, 20);

            }
            else if (hit.collider.gameObject.name == "Boss3")
            {
                ThirdBoss = true;
                bossesFinished += 1;
                gameScore += 40;
                UpdateHealth(player, 50, health, maxHealth);
                UpdateLevel(player, 75, 50, 30, 30);

            }
            else if (hit.collider.gameObject.name == "Boss4")
            {
                FourthBoss = true;
                bossesFinished += 1;
                gameScore += 50;
                UpdateHealth(player, 50, health, maxHealth);
                UpdateLevel(player, 100, 50, 40, 40);

            }
            else if (hit.collider.gameObject.name == "FinalBoss")
            {
                Final = true;
                bossesFinished += 1;
                gameScore += 100;
                UpdateHealth(player, 100, health, maxHealth);
                UpdateLevel(player, 25, 50, 50, 50);

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
        bossCount.text = bossesFinished.ToString() + "/8 Dragons Defeated";
        monsterCount.text = monstersKilled.ToString() + " Total Monsters Slain";
        goldCount.text = "You have earned" + goldCoins.ToString() + "gold";
        purchaseCount.text = "You visited [shop name] " + shopTrips.ToString() + " times and spent " + moneySpent.ToString() + "gold";
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

    public static void UpdateHealth(GameObject hero, float healthIncrease, float currenthealth, float maxHealth)
    {
        hero.GetComponent<ProtagMovement>().health += healthIncrease;
        if (currenthealth > maxHealth) currenthealth = maxHealth;
        gameScore += 5;
    }
    public static void UpdateLevel(GameObject hero, float levelThreshold, int healthMaxIncrease, float attackUpgrade, float magicUpgrade)
    {
        if (gameScore / levelThreshold == 0)
        {
            level += 1;
            hero.GetComponent<ProtagMovement>().maxHealth += healthMaxIncrease;
            UpdateAttack(hero, attackUpgrade);
            UpdateMagic(hero, magicUpgrade);
            levelThreshold *= 5;
        }
    }

    public static void UpdateAttack(GameObject hero, float attackUpgrade)
    {
        hero.GetComponent<ProtagMovement>().attackPower += attackUpgrade;
        gameScore += 5;
    }

    public static void UpdateMagic(GameObject hero, float magicUpgrade)
    {
        hero.GetComponent<ProtagMovement>().magicPower += magicUpgrade;
        gameScore += 5;
    }

    void EndGame()
    {
        SceneManager.LoadScene("WinScreen");
    }

}

