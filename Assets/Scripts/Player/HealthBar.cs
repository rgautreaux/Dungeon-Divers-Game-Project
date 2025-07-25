using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HealthBar : MonoBehaviour
{
    public GameObject healthMeter; //Assign this in the inspector
    private static Image HealthBarImage;
    private float health = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
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
        SetHealthBarValue(health / 100);

        if (health <= 0)
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }

    void OnTriggerEnter(Collider other)
    {
       object monster = gameObject.CompareTag("Monster");

        //Debug.Log("Collision with " + other.gameObject.name);
        if (other.gameObject == monster)
        {
            float damageRecieved = other.gameObject.GetComponent<Monsters>().damage;

            Debug.Log("Collision with Enemy");
            health -= damageRecieved;
            if (health < 0) health = 0;
        }
    }
    void OnTriggerStay(Collider other)
    {
        object monster = gameObject.CompareTag("Monster");

        if (other.gameObject == monster)
        {
            float damageRecieved = other.gameObject.GetComponent<Monsters>().damage;

            Debug.Log("Collision (Stay) with Enemy");
            health -= damageRecieved / 5;
            if (health < 0) health = 0;
        }
    }


}

