using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Potions : MonoBehaviour
{
    AudioSource drink;
    bool potionEffect = false;

    private Player player;
    private CharacterController characterControls;

    public GameObject potion;
    public TextMeshProUGUI potionName;

    // Start is called before the first frame update
    void Start()
    {
        drink = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
        characterControls = player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 5);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer c in allRenderers) c.enabled = false;
            Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider c in allColliders) c.enabled = false;
            StartCoroutine(PlayAndDestroy(drink.clip.length));
        }
    }

    private IEnumerator PlayAndDestroy(float waitTime)
    {
        //play fruit audio
        drink.Play();
        yield return new WaitForSeconds(waitTime);

        //select random power up
        if (!potionEffect)
        {
            if (potion.gameObject.name == "Health")
            {
                HealthPotion();
                potionEffect = true;
                Debug.Log("Drank Healing Potion");
            }
            else if (potion.gameObject.name == "Strength")
            {

                StrengthPotion();
                potionEffect = true;
                Debug.Log("Drank Strength Potion");
            }

            else if (potion.gameObject.name == "Speed")
            {

                SpeedPotion();
                potionEffect = true;
                Debug.Log("Drank Speed Potion");
            }

            else
            {
                Debug.Log("No Potion has been drank.");
            }
        }

        //destroy fruit
        Destroy(gameObject);
    }

    //double player's jump height
    void HealthPotion()
    {
        potionName.text = "Health Potion!";
        player.GetComponent<Player>().health += 5;
        characterControls.StartCoroutine(HealthEnd());
    }
    private IEnumerator HealthEnd()
    {
        yield return new WaitForSeconds(15);
        potionName.text = "   ";
        player.GetComponent<Player>().health += 0;
        potionEffect = false;
    }

    //prevent all damage to player
    void StrengthPotion()
    {
        potionName.text = "Strength Potion!";
        player.SetInvincible(true);
        player.StartCoroutine(StrengthOver());
    }
    private IEnumerator StrengthOver()
    {
        yield return new WaitForSeconds(5);
        potionName.text = "   ";
        player.SetInvincible(false);
        potionEffect = false;
    }

    //increase player's base speed
    void SpeedPotion()
    {
        potionName.text = "Speed Potion!";
        player.UpdateSpeed(5.0f);
        player.StartCoroutine(SpeedOver());
    }
    private IEnumerator SpeedOver()
    {
        yield return new WaitForSeconds(5);
        potionName.text = "   ";
        player.UpdateSpeed(2.0f);
        potionEffect = false;
    }
}