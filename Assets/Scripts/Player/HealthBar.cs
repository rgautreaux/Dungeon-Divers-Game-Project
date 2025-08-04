using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class HealthBar : MonoBehaviour
{
    public ProtagMovement player;
    public GameObject healthMeter; //Assign this in the inspector
    private static Image HealthBarImage;

    public GameObject staminaMeter; //Assign this in the inspector
    private static Image StaminaBarImage;

    private float maxHealth = 100f;
    private float health = 100f;
    private float stamina = 25f;
    private float maxStamina = 25f;

    // Start is called before the first frame update
    void Start()
    {
        health = player.GetComponent<ProtagMovement>().health;
        maxHealth = player.GetComponent<ProtagMovement>().maxHealth;

        stamina = player.GetComponent<ProtagMovement>().stamina;
        maxHealth = player.GetComponent<ProtagMovement>().maxStamina;



        if (healthMeter != null)
        {
            HealthBarImage = healthMeter.transform.GetComponent<Image>();
        }
        SetHealthBarValue(health);


        if (staminaMeter != null)
        {
            StaminaBarImage = staminaMeter.transform.GetComponent<Image>();
        }
        SetStaminaBarValue(stamina);

    }

    public static void SetHealthBarValue(float value)
    {
        HealthBarImage.fillAmount = value;
        if (HealthBarImage.fillAmount < 0.2f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (HealthBarImage.fillAmount < 0.4f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(Color.green);
        }
    }

    public static void SetStaminaBarValue(float value)
    {
        StaminaBarImage.fillAmount = value;
        if (StaminaBarImage.fillAmount < 0.2f)
        {
            SetStaminaBarColor(Color.grey);
        }
        else if (StaminaBarImage.fillAmount < 0.4f)
        {
            SetStaminaBarColor(Color.blue);
        }
        else
        {
            SetStaminaBarColor(Color.cyan);
        }
    }

    public static void SetHealthBarColor(Color staminaColor)
    {
        HealthBarImage.color = staminaColor;
    }

    public static void SetStaminaBarColor(Color staminaColor)
    {
        StaminaBarImage.color = staminaColor;
    }

    public static float GetHealthBarValue()
    {
        return HealthBarImage.fillAmount;
    }

    public static float GetStaminaBarValue()
    {
        return StaminaBarImage.fillAmount;
    }


    // Update is called once per frame
    void Update()
    {
        health = player.GetComponent<ProtagMovement>().health;
        maxHealth = player.GetComponent<ProtagMovement>().maxHealth;

        maxStamina = player.GetComponent<ProtagMovement>().maxStamina;
        stamina = player.GetComponent<ProtagMovement>().stamina;

        SetHealthBarValue(health / maxHealth);

        if (health <= 0)
        {
            SceneManager.LoadScene("LoseScreen");
        }

        SetStaminaBarValue(stamina / maxStamina);

        if (stamina <= 0)
        {
            stamina = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
       object monster = gameObject.CompareTag("Monster");
       object boss = gameObject.CompareTag("Boss");
       object breath = gameObject.GetComponent<BossBreath>().magic;


        //Debug.Log("Collision with " + other.gameObject.name);
        if (other.gameObject == monster)
        {
            float damageRecieved = other.gameObject.GetComponent<Monsters>().damage;

            Debug.Log("Collision with Enemy");
            health -= damageRecieved;
            if (health < 0) health = 0;
            if (health > maxHealth) health = maxHealth;

        }
        else if (other.gameObject == boss)
        {
            float damageRecieved = other.gameObject.GetComponent<BossScript>().damage;

            Debug.Log("Collision with Enemy");
            health -= damageRecieved;
            if (health < 0) health = 0;
            if (health > maxHealth) health = maxHealth;

        }
        else if (other.gameObject == breath)
        {
            float damageRecieved = other.gameObject.GetComponent<BossScript>().breathWeapon;

            Debug.Log("Collision with Enemy");
            health -= damageRecieved;
            if (health < 0) health = 0;
            if (health > maxHealth) health = maxHealth;

        }

    }
    void OnTriggerStay(Collider other)
    {
        object monster = gameObject.CompareTag("Monster");
        object boss = gameObject.CompareTag("Boss");
        object breath = gameObject.GetComponent<BossBreath>().magic;


        if (other.gameObject == monster)
        {
            float damageRecieved = other.gameObject.GetComponent<Monsters>().damage;

            Debug.Log("Collision (Stay) with Enemy");
            health -= damageRecieved / 5;
            if (health < 0) health = 0;
            if (health > maxHealth) health = maxHealth;


        }
        else if (other.gameObject == boss)
        {
            float damageRecieved = other.gameObject.GetComponent<BossScript>().damage;

            Debug.Log("Collision with Enemy");
            health -= damageRecieved;
            if (health < 0) health = 0;
            if (health > maxHealth) health = maxHealth;

        }
        else if (other.gameObject == breath)
        {
            float damageRecieved = other.gameObject.GetComponent<BossScript>().breathWeapon;

            Debug.Log("Collision with Enemy Magic");
            health -= damageRecieved;
            if (health < 0) health = 0;
            if (health > maxHealth) health = maxHealth;

        }
    }


}

