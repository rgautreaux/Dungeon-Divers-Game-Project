using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coins : MonoBehaviour
{
    AudioSource grab;
    private ProtagMovement player;
    private CharacterController characterControls;

    public GameObject money;

    // Start is called before the first frame update
    void Start()
    {
        grab = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ProtagMovement>();
        characterControls = player.GetComponent<ProtagMovement>().cc;
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
            StartCoroutine(PlayAndDestroy(grab.clip.length));
        }
    }

    private IEnumerator PlayAndDestroy(float waitTime)
    {
        //play audio
        grab.Play();
        yield return new WaitForSeconds(waitTime);

        if (money.gameObject.tag == "Coin")
        {
            GameStats.UpdateCurrency(5f);
            Debug.Log("Obtained Coin");
        }
        else if (money.gameObject.tag == "Chest")
        {

            GameStats.UpdateCurrency(20f);
            Debug.Log("Found a Chest");
        }
        else
        {
            Debug.Log("No Potion has been drank.");
        }

        //destroy object
        Destroy(money.gameObject);
    }
}