
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    public float breathWeapon = 0f;

    public float health = 0f;
    public GameObject self;
    public GameObject[] bossCount;


    public GameObject healthMeter; //Assign this in the inspector
    private static Image HealthBarImage;
    public new TextMeshProUGUI name;


    //Boss States
    protected BossState state = BossState.DEFAULT;
    protected Vector3 destination = new Vector3(0, 0, 0);

    Animator animator;
    AudioSource myaudio;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        name.text = self.gameObject.name;

        agent = this.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        myaudio = GetComponent<AudioSource>();

        //assign unique damage level
        if (self.gameObject.name == "SoulEater")
        {
            health = 200f;
            damage = 10f;
            breathWeapon = 20f;
        }
        else if (self.gameObject.name == "Usurper")
        {
            health = 300f;
            damage = 20f;
            breathWeapon = 30f;

        }
        else if (self.gameObject.name == "Nightmare")
        {
            health = 400f;
            damage = 30f;
            breathWeapon = 40f;

        }
        else if (self.gameObject.name == "TerrorBringer")
        {
            health = 500f;
            damage = 40f;
            breathWeapon = 50f;

        }

        if (healthMeter != null)
        {
            HealthBarImage = healthMeter.transform.GetComponent<Image>();
        }


        SetHealthBarValue(health);
        if (bossCount == null || bossCount.Length == 0)
        {
            SceneManager.LoadScene("Dungeon");
        }
    }

    public static void SetHealthBarValue(float value)
    {
        HealthBarImage.fillAmount = value;
        if (HealthBarImage.fillAmount < 0.2f)
        {
            SetHealthBarColor(Color.black);
        }
        else if (HealthBarImage.fillAmount < 0.4f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(Color.red);
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
        if (bossCount == null || bossCount.Length == 0)
        {
            SceneManager.LoadScene("Dungeon");
        }


        switch (state)
        {
            case BossState.DEFAULT:
                animator.SetBool("isIdle", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);

                GetComponent<BossBreath>().attacking = false;

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

                GetComponent<BossBreath>().attacking = false;

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

                GetComponent<BossBreath>().attacking = false;
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

                while (state == BossState.ATTACK)
                {

                    GetComponent<BossBreath>().attacking = true;
                    GameObject breath = gameObject.GetComponent<BossBreath>().magic;
                    new WaitForSeconds(15);
                    BreathAttack(breath, player);
                }

                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", false);
                if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance + 1)
                {
                    state = BossState.MOVING;
                    animator.SetBool("isAttacking", false);
                    GetComponent<BossBreath>().attacking = false;

                }
                break;
            case BossState.DEAD:

                GetComponent<BossBreath>().attacking = false;
                animator.SetBool("isDead", true);
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                break;
            default:
                break;
        }

        
    }

    public void BreathAttack(GameObject breath, GameObject player)
    {
        Collider attack = breath.GetComponent<Collider>();
        Collider target = player.GetComponent<Collider>();
        //Debug.Log("Collision with " + other.gameObject.name);
        if (attack == target)
        {
            Debug.Log("Breath Weapon Hit");
            bool potion = player.GetComponent<Potions>().potionEffect;
            float damageDealt = self.GetComponent<BossScript>().breathWeapon;

            UpdateHealth(self, 5);
            playerControls.takeDamage(damageDealt, potion);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision with " + other.gameObject.name);
        if (other.gameObject == player)
        {
            Debug.Log("Collision with Player");
            bool potion = player.GetComponent<Potions>().potionEffect;
            float damageDealt = self.GetComponent<BossScript>().damage;

            playerControls.takeDamage(damageDealt, potion);
        }
        else if (other.gameObject.CompareTag("Weapon"))
        {
            float playerAttack = ProtagMovement.attackPower;
            takeDamage(playerAttack);
            if (health < 0) health = 0;
        }
        else if (other.gameObject.CompareTag("Magic"))
        {
            float playerAttack = ProtagMovement.magicPower;
            takeDamage(playerAttack);
            if (health < 0) health = 0;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Collision with Player");
            bool potion = player.GetComponent<Potions>().potionEffect;
            float damageDealt = self.GetComponent<BossScript>().damage;

            playerControls.takeDamage(damageDealt / 5, potion);
        }
        else if (other.gameObject == gameObject.CompareTag("Weapon"))
        {
            float damageRecieved = ProtagMovement.attackPower;

            Debug.Log("Collision (Stay) with Player");
            takeDamage(damageRecieved / 5);
            if (health < 0) health = 0;
        }
        else if (other.gameObject == gameObject.CompareTag("Magic"))
        {
            float damageRecieved = ProtagMovement.magicPower;

            Debug.Log("Collision (Stay) with Magic");
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
            Destroy(healthMeter);
            Destroy(name);

        }
    }

    public static void UpdateHealth(GameObject creature, float healthIncrease)
    {
        creature.GetComponent<BossScript>().health += healthIncrease;
    }
}
