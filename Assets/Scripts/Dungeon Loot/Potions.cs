using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
public class Potions : MonoBehaviour
{
    AudioSource drink;
    public bool potionEffect = false;

    private ProtagMovement player;
    private CharacterController characterControls;

    public GameObject potion;
    public TextMeshProUGUI potionName;
    private ParticleSystem effect;

    //Detecting monsters
    GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        drink = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ProtagMovement>();
        characterControls = player.GetComponent<ProtagMovement>().cc;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 5);
        monster = GameObject.FindWithTag("Monster");

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
        //play potion audio
        drink.Play();
        yield return new WaitForSeconds(waitTime);

        //select power up
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
                potionEffect = false;
                Debug.Log("No Potion has been drank.");
            }
        }

        //destroy potion
        Destroy(gameObject);
    }

    //increase health
    void HealthPotion()
    {
        potionName.text = "Health Potion!";
        effect = player.GetComponent<ProtagMovement>().healthMagic;
        effect.Play();
        player.GetComponent<ProtagMovement>().health += 5;
        player.StartCoroutine(HealthEnd());
    }
    private IEnumerator HealthEnd()
    {
        yield return new WaitForSeconds(15);
        potionName.text = "   ";
        effect.Stop();
        player.GetComponent<ProtagMovement>().health += 0;
        potionEffect = false;
    }

    //reduce damage to player
    void StrengthPotion()
    {
        float damage = monster.GetComponent<Monsters>().damage;
        potionName.text = "Strength Potion!";
        effect = player.GetComponent<ProtagMovement>().strengthMagic;
        effect.Play();
        player.takeDamage(damage, true);
        player.StartCoroutine(StrengthOver());
    }
    private IEnumerator StrengthOver()
    {
        float damage = monster.GetComponent<Monsters>().damage;
        yield return new WaitForSeconds(10);
        potionName.text = "   ";
        effect = player.GetComponent<ProtagMovement>().strengthMagic;
        effect.Stop();
        player.takeDamage(damage, false);
        potionEffect = false;
    }

    //increase player's base speed
    void SpeedPotion()
    {
        potionName.text = "Speed Potion!";
        effect = player.GetComponent<ProtagMovement>().speedMagic;
        effect.Play();
        player.UpdateSpeed(10f);
        player.StartCoroutine(SpeedOver());
    }
    private IEnumerator SpeedOver()
    {
        yield return new WaitForSeconds(5);
        potionName.text = "   ";
        effect = player.GetComponent<ProtagMovement>().speedMagic;
        effect.Stop();
        player.UpdateSpeed(5f);
        potionEffect = false;
    }
}