using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    public GameObject[] Loot;
    public GameObject coins;
    public GameObject chest;
    public GameObject[] potions;
    private GameObject parentObject;
    public int maxCoinsCount = 100;
    public int maxChestCount = 5;
    public int maxPotionCount = 25;

    public float coinSpawnBreak = 5;
    public float chestSpawnBreak = 30;
    public float potionSpawnBreak = 15;


    float elapsedCoinTime = 0;
    float elapsedChestTime = 0;
    float elapsedPotionTime = 0;

    int currentCoinCount = 0;
    int currentChestCount = 0;
    int currentPotionCount = 0;

    int numberofLootPrefabs = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentCoinCount = CountCoins();
        currentChestCount = CountChests();
        currentPotionCount = CountPotions();

        parentObject = transform.gameObject;
        numberofLootPrefabs = Loot.Length;

    }

    // Update is called once per frame
    void Update()
    {
        elapsedCoinTime += Time.deltaTime;
        elapsedChestTime += Time.deltaTime;
        elapsedPotionTime += Time.deltaTime;

        if (elapsedCoinTime > coinSpawnBreak && currentCoinCount < maxCoinsCount)
        {
            elapsedCoinTime = 0;
            Vector3 spawnPosition = RandomPositionAroundPlayer();
            int randomIndex = Random.Range(0, numberofLootPrefabs);
            GameObject newLoot = (GameObject)Instantiate(Loot[randomIndex], spawnPosition, Quaternion.Euler(0, 0, 0));
            newLoot.transform.SetParent(parentObject.transform);
            currentCoinCount = CountCoins();
        }
        else if (elapsedChestTime > chestSpawnBreak && currentChestCount < maxChestCount)
        {
            elapsedChestTime = 0;
            Vector3 spawnPosition = RandomPositionAroundPlayer();
            int randomIndex = Random.Range(0, numberofLootPrefabs);
            GameObject newLoot = (GameObject)Instantiate(Loot[randomIndex], spawnPosition, Quaternion.Euler(0, 0, 0));
            newLoot.transform.SetParent(parentObject.transform);
            currentChestCount = CountChests();
        }
        else if (elapsedPotionTime > potionSpawnBreak && currentPotionCount < maxPotionCount)
        {
            elapsedPotionTime = 0;
            Vector3 spawnPosition = RandomPositionAroundPlayer();
            int randomIndex = Random.Range(0, numberofLootPrefabs);
            GameObject newLoot = (GameObject)Instantiate(Loot[randomIndex], spawnPosition, Quaternion.Euler(0, 0, 0));
            newLoot.transform.SetParent(parentObject.transform);
            currentPotionCount = CountPotions();
        }

    }

    private int CountCoins()
    {
        int count = 0;
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        count = coins.Length;
        return count;
    }
    private int CountChests()
    {
        int count = 0;
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        count = chests.Length;
        return count;
    }
    private int CountPotions()
    {
        int count = 0;
        GameObject[] potions = GameObject.FindGameObjectsWithTag("Potion");
        count = potions.Length;
        return count;
    }
    private Vector3 RandomPositionAroundPlayer()
    {
        Vector3 randPos = new Vector3(Random.Range(-30.0f, 30.0f), 0, Random.Range(-30.0f, 30.0f));
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        randPos += playerPos;
        randPos.x += 10.0f;
        randPos.y = 0.11f;
        randPos.z += 10.0f;
        return randPos;
    }

}

