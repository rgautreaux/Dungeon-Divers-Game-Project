
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BossState { ATTACK, CHASE, MOVING, DEAD, DEFAULT };

[RequireComponent(typeof(NavMeshAgent))]

public class BossScript : MonoBehaviour
{
    //Player Data
    GameObject player;
    ProtagMovement playerControls;

    //Boss Stats
    NavMeshAgent agent;
    public float chaseDistance = 10.0f;
    public float damage = 0f;
    public float health = 0f;
    public GameObject self;

    public GameObject healthMeter; //Assign this in the inspector
    private static Image HealthBarImage;

    //Boss States
    protected BossState state = BossState.DEFAULT;
    protected Vector3 destination = new Vector3(0, 0, 0);

    Animator animator;
    AudioSource myaudio;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        self = this.gameObject;

        agent = this.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        myaudio = GetComponent<AudioSource>();


        //assign unique damage level
        if (self.gameObject.name == "SoulEater")
        {
            health = 200f;
            damage = 10f;
        }
        else if (self.gameObject.name == "Usurper")
        {
            health = 300f;
            damage = 20f;
        }
        else if (self.gameObject.name == "Nightmare")
        {
            health = 400f;
            damage = 30f;
        }
        else if (self.gameObject.name == "TerrorBringer")
        {
            health = 500f;
            damage = 40f;
        }

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

        switch (state)
        {
            case BossState.DEFAULT:
                animator.SetBool("isIdle", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = BossState.CHASE;
                }
                else
                {
                    state = BossState.MOVING;
                    destination = player.transform.position;
                    agent.SetDestination(destination);
                }
                break;
            case BossState.MOVING:
                animator.SetBool("isMoving", true);
                //Debug.Log("Dest = " + destination + "Distance = " + Vector3.Distance(transform.position, destination));
                if (Vector3.Distance(transform.position, destination) < 2.0f)
                {
                    state = BossState.DEFAULT;
                }

                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = BossState.CHASE;
                }
                break;
            case BossState.CHASE:
                if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance)
                {
                    state = BossState.DEFAULT;
                }
                agent.SetDestination(player.transform.position);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", true);
                if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
                {
                    state = BossState.ATTACK;
                }
                break;
            case BossState.ATTACK:
                animator.SetBool("isAttacking", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", false);
                if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance + 1)
                {
                    state = BossState.MOVING;
                    animator.SetBool("isAttacking", false);
                }
                break;
            case    BossState.DEAD:
                animator.SetBool("isDead", true);
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision with " + other.gameObject.name);
        if (other.gameObject == player)
        {
            float damageRecieved = other.gameObject.GetComponent<Monsters>().damage;

            Debug.Log("Collision with Player");
            bool potion = player.GetComponent<Potions>().potionEffect;
            float damageDealt = self.GetComponent<BossScript>().damage;

            playerControls.takeDamage(damageDealt, potion); 
        }
        else if (other.gameObject.CompareTag("Weapon"))
        {
            float playerAttack = player.GetComponent<ProtagMovement>().attackPower;
            takeDamage(playerAttack);
            if (health < 0) health = 0;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            float damageRecieved = other.gameObject.GetComponent<Monsters>().damage;

            Debug.Log("Collision with Player");
            bool potion = player.GetComponent<Potions>().potionEffect;
            float damageDealt = self.GetComponent<BossScript>().damage;

            playerControls.takeDamage(damageDealt / 5, potion);
        }
        else if (other.gameObject == gameObject.CompareTag("Weapon"))
        {
            float damageRecieved = player.gameObject.GetComponent<ProtagMovement>().attackPower;

            Debug.Log("Collision (Stay) with Enemy");
            takeDamage(damageRecieved / 5);
            if (health < 0) health = 0;
        }
    }

    private IEnumerator PlayAndDestroy(float waitTime)
    {
        myaudio.Play();
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    public void takeDamage(float playerAttack)
    {
        //Base Damage

        health -= playerAttack;
        if (health < 0) health = 0;

        Debug.Log("Player dealt " + playerAttack + "damage, " + health + "HP remain.");

        if (health <= 0)
        {
            // Disable all Renderers and Colliders
            //Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
            //foreach (Renderer c in allRenderers) c.enabled = false;
            Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider c in allColliders) c.enabled = false;

            state = BossState.DEAD;
            StartCoroutine(PlayAndDestroy(3.0f));
            GameStats.UpdateDragonsKilled();
        }
    }

}

