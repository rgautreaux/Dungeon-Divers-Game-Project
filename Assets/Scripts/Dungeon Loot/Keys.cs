using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keys : MonoBehaviour
{
    AudioSource unlock;
    private ProtagMovement player;
    private CharacterController characterControls;

    public GameObject Key;


    // Start is called before the first frame update
    void Start()
    {
        unlock = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ProtagMovement>();
        characterControls = player.GetComponent<ProtagMovement>().cc;
    }

    // Update is called once per frame
    void Update()
    {
        Key.transform.Rotate(0, 0, 5);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer c in allRenderers) c.enabled = false;
            Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider c in allColliders) c.enabled = false;
            StartCoroutine(PlayAndDestroy(unlock.clip.length));
        }
    }

    private IEnumerator PlayAndDestroy(float waitTime)
    {
        //play audio
        unlock.Play();
        yield return new WaitForSeconds(waitTime);

        if (Key.gameObject.name == "SECTION2KEY")
        {
            GameObject[] Section2Doors = GameObject.FindGameObjectsWithTag("Section 2");
            foreach (GameObject c in Section2Doors)
            {
                Destroy(c);
            }
            Debug.Log("Section 2 Unlocked");
        }
        else if (Key.gameObject.name == "SECTION3KEY")
        {

            GameObject[] Section3Doors = GameObject.FindGameObjectsWithTag("Section 3");
            foreach (GameObject c in Section3Doors)
            {
                Destroy(c);
            }
            Debug.Log("Section 3 Unlocked");
        }
        else if (Key.gameObject.name == "SECTION4KEY")
        {

            GameObject[] Section4Doors = GameObject.FindGameObjectsWithTag("Section 4");
            foreach (GameObject c in Section4Doors)
            {
                Destroy(c);
            }
            Debug.Log("Section 4 Unlocked");
        }
        else
        {
            Debug.Log("No Section is Unlocked yet");
        }

        //destroy object
        Destroy(Key.gameObject);
    }
}