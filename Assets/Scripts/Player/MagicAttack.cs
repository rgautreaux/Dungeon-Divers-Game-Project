using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MagicAttack : MonoBehaviour
{
    //Magic Prefab
    public GameObject magic;

    //Speed of Magic
    public float magicForce = 500.0f;

    //Destroy time
    public float destroyTime = 3.0f;

    AudioSource myaudio;

    public ParticleSystem magicPower;

    // Start is called before the first frame update
    void Start()
    {
        myaudio = GetComponent<AudioSource>();
        magicPower = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //create a magic instance
            GameObject castMagic = Instantiate(magic, this.transform.position, this.transform.rotation) as GameObject;

            //fix scale
            castMagic.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            //add force to shoot
            castMagic.GetComponent<Rigidbody>().AddForce(transform.forward * magicForce);
            magicPower.Play();
            myaudio.Play();

            //Destroy it after a certain time
            Destroy(castMagic, destroyTime);
        }

    }
}