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

    public GameObject Gal;
    public GameObject Guy;

    public GameObject galButton;
    public GameObject guyButton;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit pressButton;

        // Interact with knobs and switches
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out pressButton))
        {
            if (pressButton.collider.CompareTag("Gal"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
                {
                    // sound effect
                    GalButton();
                    Debug.Log("Fem Adventurer Selected");
                }
            }
            if (pressButton.collider.CompareTag("Guy"))
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
        GameObject selected = Gal;
        GetComponent<ProtagMovement>().playerChara = selected;

        SceneManager.LoadScene("Dungeon");
    }

    public void GuyButton()
    {
        GameObject selected = Guy;
        GetComponent<ProtagMovement>().playerChara = selected;

        SceneManager.LoadScene("Dungeon");
    }

}