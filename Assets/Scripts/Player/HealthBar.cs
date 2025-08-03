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
    private float maxHealth = 100f;
    private float health = 100f;

    // Start is called before the first frame update
    void Start()
    {
        health = player.GetComponent<ProtagMovement>().health;
        maxHealth = player.GetComponent<ProtagMovement>().maxHealth;

        if (healthMeter != null)
        {
            HealthBarImage = healthMeter.transform.GetComponent<Image>();
        }
        SetHealthBarValue(health);

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

    public static void SetHealthBarColor(Color healthColor)
    {
        HealthBarImage.color = healthColor;
    }

    public static float GetHealthBarValue()
    {
        return HealthBarImage.fillAmount;
    }


    // Update is called once per frame
    void Update()
    {
        health = player.GetComponent<ProtagMovement>().health;
        maxHealth = player.GetComponent<ProtagMovement>().maxHealth;

        SetHealthBarValue(health / maxHealth);

        if (health <= 0)
        {
            SceneManager.LoadScene("LoseScreen");
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

