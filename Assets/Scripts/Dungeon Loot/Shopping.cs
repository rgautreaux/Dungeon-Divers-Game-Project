using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Shopping : MonoBehaviour
{
    public RaycastHit select;
    private Camera camera;
    public TextMeshProUGUI Dialogue;
    public TextMeshProUGUI bank;

    AudioSource purchase;

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

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        purchase = GetComponent<AudioSource>();

        bank.text = GameStats.goldCoins.ToString() +" GLD";

        addAttackVal.text = "+" + addAttack.ToString() + "ATTK (" + attkCount.ToString() + ")";
        addArmorVal.text = "+" + addArmor.ToString() + "DEF (" + armorCount.ToString() + ")";
        addMagicVal.text = "+" + addMagic.ToString() + "MAGIC (" + magicCount.ToString() + ")";
        hPotionVal.text = "+HEAL (" + healthCount.ToString() + ")";
        sPotionVal.text = "+SPEED (" + speedCount.ToString() + ")";
        bPotionVal.text = "+STR (" + strengthCount.ToString() + ")";

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

        RaycastHit pressButton;

        // Interact with buttons
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out pressButton))
        {
            if (pressButton.collider.CompareTag("Sword") || pressButton.collider.gameObject == attackPwrButton)
            {
                Dialogue.text = "Looking to increase your " + ProtagMovement.attackPower.ToString() + " Sword Sharpness by " + addAttack.ToString() + " for " + attackPrice.ToString() + "?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 || GameStats.goldCoins < attackPrice)
                    {
                        purchase.Play();
                        BuyAttack();
                        Debug.Log("UPGRADE ATTACK");
                    }
                    else
                    {
                        Dialogue.text = " ";

                    }
                }
            }
            if (pressButton.collider.CompareTag("Shield") || pressButton.collider.gameObject == armorPwrButton)
            {
                Dialogue.text = "Looking to increase your " + ProtagMovement.armorPower.ToString() + " Armor Strength by " + addArmor.ToString() + " for " + armorPrice.ToString() + "?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 || GameStats.goldCoins < armorPrice)
                    {
                        purchase.Play();
                        BuyArmor();
                        Debug.Log("UPGRADE ARMOR");
                    }
                    else
                    {
                        Dialogue.text = " ";
                    }
                }
            }
            if (pressButton.collider.CompareTag("Spell") || pressButton.collider.gameObject == magicPwrButton)
            {
                Dialogue.text = "Looking to increase your " + ProtagMovement.magicPower.ToString() + " Magic Strength by " + addMagic.ToString() + " for " + magicPrice.ToString() + "?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 || GameStats.goldCoins < magicPrice)
                    {
                        purchase.Play();
                        BuyMagic();
                        Debug.Log("UPGRADE MAGIC");
                    }
                    else
                    {
                        Dialogue.text = " ";

                    }
                }
            }
            if (pressButton.collider.CompareTag("Health") || pressButton.collider.gameObject == HealthPotionButton)
            {
                Dialogue.text = "Considering a Health Potion this time? They're " + potionPrice.ToString() + "Gold.";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 || GameStats.goldCoins < potionPrice)
                    {
                        purchase.Play();
                        BuyHPotion();
                        Debug.Log("HEALTH POTION");
                    }
                    else
                    {
                        Dialogue.text = " ";

                    }
                }
            }
            if (pressButton.collider.CompareTag("Speed") || pressButton.collider.gameObject == SpeedPotionButton)
            {
                Dialogue.text = "Considering a Speed Potion this time?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 || GameStats.goldCoins < potionPrice)
                    {
                        purchase.Play();
                        BuySPotion();
                        Debug.Log("SPEED POTION");
                    }
                    else
                    {
                        Dialogue.text = " ";

                    }
                }
            }
            if (pressButton.collider.CompareTag("Strength") || pressButton.collider.gameObject == StrengthPotionButton)
            {
                Dialogue.text = "Considering a Strength Potion this time?";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    if (GameStats.goldCoins > 0 || GameStats.goldCoins < potionPrice)
                    {
                        purchase.Play();
                        BuyBPotion();
                        Debug.Log("STRENGTH POTION");
                    }
                    else
                    {
                        Dialogue.text = " ";

                    }
                }
            }
            if (pressButton.collider.CompareTag("Exit") || pressButton.collider.gameObject == EXIT)
            {
                Dialogue.text = " ";

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    ExitShop();
                    Debug.Log("Leaving Shop");
                }
            }
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


    public void BuyAttack()
    {
        GameStats.UpdateAttack(addAttack);
        GameStats.goldCoins -= attackPrice;
        GameStats.moneySpent += attackPrice;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;
    }
    public void BuyArmor()
    {
        GameStats.UpdateArmor(addArmor);
        GameStats.goldCoins -= armorPrice;
        GameStats.moneySpent += armorPrice;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void BuyMagic()
    {
        GameStats.UpdateMagic(addMagic);
        GameStats.goldCoins -= magicPrice;
        GameStats.moneySpent += magicPrice;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void BuyHPotion()
    {
        drinkHealth = true;
        GameStats.goldCoins -= potionPrice;
        GameStats.moneySpent += potionPrice;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void BuySPotion()
    {
        drinkSpeed = true;
        GameStats.goldCoins -= potionPrice;
        GameStats.moneySpent += potionPrice;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void BuyBPotion()
    {
        drinkStrength = true;
        GameStats.goldCoins -= potionPrice;
        GameStats.moneySpent += potionPrice;

        if (GameStats.goldCoins < 0) GameStats.goldCoins = 0;


    }
    public void ExitShop()
    {
        SceneManager.LoadScene("Dungeon");
    }
}