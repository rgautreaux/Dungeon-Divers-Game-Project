using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class CharaSelect : MonoBehaviour
{
    public RaycastHit select;
    private Camera camera;
    public AudioSource selectMusic;
    public GameObject Gal;
    public GameObject Guy;

    public GameObject galButton;
    public GameObject guyButton;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        selectMusic.playOnAwake = true;
        selectMusic.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        //create ray
        Ray mouseCursor = camera.ScreenPointToRay(Input.mousePosition);

        // Interact with buttons
        if (Physics.Raycast(mouseCursor, out RaycastHit pressButton, 10f))
        {
            if (pressButton.collider.CompareTag("PickGal") || pressButton.collider.gameObject == galButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    GalButton();
                    Debug.Log("Fem Adventurer Selected");
                }
            }
            if (pressButton.collider.CompareTag("PickGuy") || pressButton.collider.gameObject == guyButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    GuyButton();
                    Debug.Log("Masc Adventurer Selected");
                }
            }
        }
    }

    public void GalButton()
    {
        ProtagMovement.galPicked = true;
        selectMusic.Stop();
        SceneManager.LoadScene("Dungeon");
    }

    public void GuyButton()
    {
        ProtagMovement.guyPicked = true;
        selectMusic.Stop();
        SceneManager.LoadScene("Dungeon");
    }

}