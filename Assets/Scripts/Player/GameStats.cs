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

        bossCount.text = bossesFinished.ToString() + "/5 Boss Fights Defeated";
        monsterCount.text = monstersKilled.ToString() + " Total Monsters Slain";
        goldCount.text = "You have earned" + goldCoins.ToString() + "gold";
        purchaseCount.text = "You visited [shop name] " + shopTrips.ToString() + " times and spent " + moneySpent.ToString() + "gold";
        gameScoreText.text = "Game Score = " + gameScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Create raycast from camera to check for interactables
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject.name  == "Boss1")
            {
                FirstBoss = true;
                bossesFinished += 1;
                gameScore += 20;

                if (gameScore % 15 == 0)
                {
                    level += 1;
                    player.GetComponent<ProtagMovement>().health += 50;

                }

            }
            else if (hit.collider.gameObject.name == "Boss2")
            {
                SecBoss = true;
                bossesFinished += 1;
                gameScore += 30;

                if (gameScore % 20 == 0)
                {
                    level += 1;
                    player.GetComponent<ProtagMovement>().health += 50;

                }

            }
            else if (hit.collider.gameObject.name == "Boss3")
            {
                ThirdBoss = true;
                bossesFinished += 1;
                gameScore += 40;

                if (gameScore % 25 == 0)
                {
                    level += 1;
                    player.GetComponent<ProtagMovement>().health += 50;

                }

            }
            else if (hit.collider.gameObject.name == "Boss4")
            {
                FourthBoss = true;
                bossesFinished += 1;
                gameScore += 50;

                if (gameScore % 30 == 0)
                {
                    level += 1;
                    player.GetComponent<ProtagMovement>().health += 50;

                }
            }
            else if (hit.collider.gameObject.name == "Final")
            {
                Final = true;
                bossesFinished += 1;
                gameScore += 100;

                if (gameScore % 35 == 0)
                {
                    level += 1;
                    player.GetComponent<ProtagMovement>().health += 50;

                }
            }
            else
            {
                if (gameScore % 10 == 0)
                {
                    level += 1;
                    player.GetComponent<ProtagMovement>().health += 50;

                }
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
        bossCount.text = bossesFinished.ToString() + "/5 Boss Fights Defeated";
        monsterCount.text = monstersKilled.ToString() + " Total Monsters Slain";
        goldCount.text = "You have earned" + goldCoins.ToString() + "gold";
        purchaseCount.text = "You visited [shop name] " + shopTrips.ToString() + " times and spent " + moneySpent.ToString() + "gold";
        gameScoreText.text = "Game Score = " + gameScore.ToString();
    

        if (bossesFinished >= 5)
        {
            EndGame();
        }
    }

    public static void UpdateMonstersKilled()
    {
        monstersKilled += 1;
        gameScore += 15;
    }

    public static void UpdateDragonsKilled()
    {
        bossesFinished += 1;
        gameScore += 15;
    }

    public static void UpdateHealth()
    {
        monstersKilled += 1;
        gameScore += 15;
    }

    void EndGame()
    {
        SceneManager.LoadScene("WinScreen");
    }

}

