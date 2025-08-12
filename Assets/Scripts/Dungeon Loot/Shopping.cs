using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using UnityEditor;

public class Shopping : MonoBehaviour
{
    public RaycastHit select;
    private Camera camera;
    public TextMeshProUGUI bank;
    AudioSource purchase;
    public AudioSource shopSound;


    private string[] greeting = { "Welcome back! How's the dungeon crawl treatin ya?", "Hello again. You lasted a lot longer than I thought you would.", "Well look what the Dragon dragged in.. what can I do for ya?", "Been a bit buddy, how's it hanging?", "Was worried you'd be lookin like me next time I saw ya, glad you're in one piece.", "Find any dragons yet? I think you got a fightin chance."};
    private string[] rejection = { "No can do pal.", "Sorry buddy, but can't do that.", "Maybe next time but.. no.", "Nice try, but no can do pal.", "Sorry to say, but that's not happening.", "Are you pulling my femur? No chance.", "Are you serious? Ain't happening pal.", "Don't make me throw ya out, I'm trying to run a bone-a-fide busniess here buddy!", "No way pal, don't even try that."};
    private string[] pun = { "Hope exploring this dungeon hasn't left you too rattled.", "I'll have you know the this place is the only place you'll find these bone-a fide finds.", "This is going tibia long time before you get to the end, but don't give up!", "Not a fan of my jokes? I found them humerus.. but maybe your sense of humor is the more bone-dry joke variety."};
    private string[] goodbye = { "Good luck out there!", "See ya later! Maybe.. hopefully!", "Rootin for ya pal!", "Try not to become Dragon Chow!", "You know where to find me.", "Byeeeeeeeeee!", "Best of luck pal!", "Knock 'em dead buddy!", "Bring back some gold and I'll try to find somthing good for ya.", "Go on and rattle 'em pal!"};

    public TextMeshProUGUI Dialogue;

    public GameObject EXIT;
    public GameObject attackPwrButton;
    public GameObject armorPwrButton;
    public GameObject magicPwrButton;
    public GameObject HealthPotionButton;
    public GameObject SpeedPotionButton;
    public GameObject StrengthPotionButton;

    public static float addAttack = 2;
    public TextMeshProUGUI addAttackVal;

    public static float addArmor = 2;
    public TextMeshProUGUI addArmorVal;

    public static float addMagic = 2;
    public TextMeshProUGUI addMagicVal;


    public static bool drinkHealth = false;
    public TextMeshProUGUI hPotionVal;

    public static bool drinkSpeed = false;
    public TextMeshProUGUI sPotionVal;

    public static bool drinkStrength = false;
    public TextMeshProUGUI bPotionVal;

    public static int attackPrice = 10;
    public static int armorPrice = 15;
    public static int magicPrice = 20;
    public static int potionPrice = 30;  

    public float attkCount = 0;
    public float armorCount = 0;
    public float magicCount = 0;
    public static float healthCount = 0;
    public static float speedCount = 0;
    public static float strengthCount = 0;

    public static float itemLimit = 10;
    public int patience = 3;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        purchase = GetComponent<AudioSource>();
        shopSound.playOnAwake = true;
        shopSound.loop = true;

        bank.text = GameStats.goldCoins.ToString() + " GLD";

        addAttackVal.text = "+" + addAttack.ToString() + "ATTK (" + attkCount.ToString() + ")";
        addArmorVal.text = "+" + addArmor.ToString() + "DEF (" + armorCount.ToString() + ")";
        addMagicVal.text = "+" + addMagic.ToString() + "MAGIC (" + magicCount.ToString() + ")";
        hPotionVal.text = "+HEAL (" + healthCount.ToString() + ")";
        sPotionVal.text = "+SPEED (" + speedCount.ToString() + ")";
        bPotionVal.text = "+STR (" + strengthCount.ToString() + ")";

        if (GameStats.shopTrips == 1 && GameStats.totalTimesPlayed <= 1) 
        {
            Dialogue.text = "Welcome, newcomer, to the Undead Deals! Name's Skullivan Marrow, but my pals call me Skully. Its nice to see a fresh face around here, hope you last longer than the last guy...";
            new WaitForSeconds(10);
            Dialogue.text = "Welp, let me know if anything suits your fancy.";

        }
        if (GameStats.shopTrips == 1 && GameStats.totalTimesPlayed > 1)
        {
            Dialogue.text = "Welcome, newcomer, to the Undead Deals! Name's Skully, but you... look familiar... Must be deja vu!";
            new WaitForSeconds(10);
            Dialogue.text = "Anyway, let me know if anything suits your fancy pal.";

        }
        else 
        {
            string hello = Greeting();
            Dialogue.text = hello;
        }

        patience = 3;

        attkCount = Random.Range(1, itemLimit);
        armorCount = Random.Range(1, itemLimit);
        magicCount = Random.Range(1, itemLimit);
        healthCount = Random.Range(1, itemLimit);
        speedCount = Random.Range(1, itemLimit);
        strengthCount = Random.Range(1, itemLimit);
    }

    // Update is called once per frame
    void Update()
    {
        bank.text = GameStats.goldCoins.ToString() + " GLD";

        addAttackVal.text = "+" + addAttack.ToString() + "ATTK (" + attkCount.ToString() + ")";
        addArmorVal.text = "+" + addArmor.ToString() + "DEF (" + armorCount.ToString() + ")";
        addMagicVal.text = "+" + addMagic.ToString() + "MAGIC (" + magicCount.ToString() + ")";
        hPotionVal.text = "+HEAL (" + healthCount.ToString() + ")";
        sPotionVal.text = "+SPEED (" + speedCount.ToString() + ")";
        bPotionVal.text = "+STR (" + strengthCount.ToString() + ")";

        //create ray
        Ray mouseCursor = camera.ScreenPointToRay(Input.mousePosition);

        // Interact with buttons
        if (Physics.Raycast(mouseCursor, out RaycastHit pressButton, 10f))
        {
            if (pressButton.collider.CompareTag("Sword") || pressButton.collider.gameObject == attackPwrButton)
            {
                Dialogue.text = "Looking to increase your " + ProtagMovement.attackPower.ToString() + " Sword Sharpness by " + addAttack.ToString() + " for " + attackPrice.ToString() + "?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 && GameStats.goldCoins >= attackPrice && attkCount > 0)
                    {
                        purchase.Play();
                        BuyAttack();
                        Debug.Log("UPGRADE ATTACK");
                    }
                    else
                    {
                        string no = Nope();
                        Dialogue.text = no;
                        patience -= 1;
                        new WaitForSeconds(10);
                        string pun = Joke();
                        Dialogue.text = pun;

                    }
                }
            }
            if (pressButton.collider.CompareTag("Shield") || pressButton.collider.gameObject == armorPwrButton)
            {
                Dialogue.text = "Looking to increase your " + ProtagMovement.armorPower.ToString() + " Armor Strength by " + addArmor.ToString() + " for " + armorPrice.ToString() + "?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 && GameStats.goldCoins >= armorPrice && armorCount > 0)
                    {
                        purchase.Play();
                        BuyArmor();
                        Debug.Log("UPGRADE ARMOR");
                    }
                    else
                    {
                        string no = Nope();
                        Dialogue.text = no;
                        patience -= 1;
                        new WaitForSeconds(10);
                        string pun = Joke();
                        Dialogue.text = pun;

                    }
                }
            }
            if (pressButton.collider.CompareTag("Spell") || pressButton.collider.gameObject == magicPwrButton)
            {
                Dialogue.text = "Looking to increase your " + ProtagMovement.magicPower.ToString() + " Magic Strength by " + addMagic.ToString() + " for " + magicPrice.ToString() + "?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 && GameStats.goldCoins >= magicPrice && magicCount > 0)
                    {
                        purchase.Play();
                        BuyMagic();
                        Debug.Log("UPGRADE MAGIC");
                    }
                    else
                    {
                        string no = Nope();
                        Dialogue.text = no;
                        patience -= 1;
                        new WaitForSeconds(10);
                        string pun = Joke();
                        Dialogue.text = pun;
                    }
                }
            }
            if (pressButton.collider.CompareTag("Health") || pressButton.collider.gameObject == HealthPotionButton)
            {
                Dialogue.text = "Considering a Health Potion this time? They're " + potionPrice.ToString() + "Gold.";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 && GameStats.goldCoins >= potionPrice && healthCount > 0)
                    {
                        purchase.Play();
                        BuyHPotion();
                        Debug.Log("HEALTH POTION");
                    }
                    else
                    {
                        string no = Nope();
                        Dialogue.text = no;
                        patience -= 1;
                        new WaitForSeconds(10);
                        string pun = Joke();
                        Dialogue.text = pun;
                    }
                }
            }
            if (pressButton.collider.CompareTag("Speed") || pressButton.collider.gameObject == SpeedPotionButton)
            {
                Dialogue.text = "Considering a Speed Potion this time?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 && GameStats.goldCoins >= potionPrice && speedCount > 0)
                    {
                        purchase.Play();
                        BuySPotion();
                        Debug.Log("SPEED POTION");
                    }
                    else
                    {
                        string no = Nope();
                        Dialogue.text = no;
                        patience -= 1;
                        new WaitForSeconds(10);
                        string pun = Joke();
                        Dialogue.text = pun;
                    }
                }
            }
            if (pressButton.collider.CompareTag("Strength") || pressButton.collider.gameObject == StrengthPotionButton)
            {
                Dialogue.text = "Considering a Strength Potion this time?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 && GameStats.goldCoins >= potionPrice && strengthCount > 0)
                    {
                        purchase.Play();
                        BuyBPotion();
                        Debug.Log("STRENGTH POTION");
                    }
                    else
                    {
                        string no = Nope();
                        Dialogue.text = no;
                        patience -= 1;
                        new WaitForSeconds(10);
                        string pun = Joke();
                        Dialogue.text = pun;
                    }
                }
            }
            if (pressButton.collider.CompareTag("Exit") || pressButton.collider.gameObject == EXIT)
            {

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    string bye = Goodbye();
                    Dialogue.text = bye;
                    ExitShop();
                    Debug.Log("Leaving Shop");
                }
            }
        }

        if (patience <= 0)
        {
            GameStats.health -= 5;
            string bye = Goodbye();
            Dialogue.text = bye;
            ExitShop();
            Debug.Log("Leaving Shop");
        }
    }

    private void OnGUI()
    {

        bank.text = GameStats.goldCoins.ToString() + " GLD";

        addAttackVal.text = "+" + addAttack.ToString() + "ATTK (" + attkCount.ToString() + ")";
        addArmorVal.text = "+" + addArmor.ToString() + "DEF (" + armorCount.ToString() + ")";
        addMagicVal.text = "+" + addMagic.ToString() + "MAGIC (" + magicCount.ToString() + ")";
        hPotionVal.text = "+HEAL (" + healthCount.ToString() + ")";
        sPotionVal.text = "+SPEED (" + speedCount.ToString() + ")";
        bPotionVal.text = "+STR (" + strengthCount.ToString() + ")";
    }

    private string Greeting()
    {
        // grab a random string from the words array
        string randomWord = greeting[Random.Range(0, greeting.Length)];

        // return it (this will be the string that the script will use)
        return randomWord;
    }

    private string Goodbye()
    {
        // grab a random string from the words array
        string randomWord = goodbye[Random.Range(0, goodbye.Length)];

        // return it (this will be the string that the script will use)
        return randomWord;
    }

    private string Nope()
    {
        // grab a random string from the words array
        string randomWord = rejection[Random.Range(0, rejection.Length)];

        // return it (this will be the string that the script will use)
        return randomWord;
    }

    private string Joke()
    {
        // grab a random string from the words array
        string randomWord = pun[Random.Range(0, pun.Length)];

        // return it (this will be the string that the script will use)
        return randomWord;
    }

    public void BuyAttack()
    {
        GameStats.UpdateAttack(addAttack);
        GameStats.goldCoins -= attackPrice;
        GameStats.moneySpent += attackPrice;
        attkCount -= 1;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;
    }
    public void BuyArmor()
    {
        GameStats.UpdateArmor(addArmor);
        GameStats.goldCoins -= armorPrice;
        GameStats.moneySpent += armorPrice;
        armorCount -= 1;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void BuyMagic()
    {
        GameStats.UpdateMagic(addMagic);
        GameStats.goldCoins -= magicPrice;
        GameStats.moneySpent += magicPrice;
        magicCount -= 1;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void BuyHPotion()
    {
        drinkHealth = true;
        GameStats.goldCoins -= potionPrice;
        GameStats.moneySpent += potionPrice;
        healthCount -= 1;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void BuySPotion()
    {
        drinkSpeed = true;
        GameStats.goldCoins -= potionPrice;
        GameStats.moneySpent += potionPrice;
        speedCount -= 1;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void BuyBPotion()
    {
        drinkStrength = true;
        GameStats.goldCoins -= potionPrice;
        GameStats.moneySpent += potionPrice;
        strengthCount -= 1;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void ExitShop()
    {
        new WaitForSeconds(15);
        shopSound.Stop();
        SceneManager.LoadScene("Dungeon");
    }
}