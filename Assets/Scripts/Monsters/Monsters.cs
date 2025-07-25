
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum MonsterState { ATTACK, CHASE, MOVING, DEAD, DEFAULT };

[RequireComponent(typeof(NavMeshAgent))]

public class Monsters : MonoBehaviour
{
    GameObject player;
    ProtagMovement playerControls;
    NavMeshAgent agent;
    public float chaseDistance = 10.0f;
    public float damage = 0f;
    public GameObject self;

    protected MonsterState state = MonsterState.DEFAULT;
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
        if (self.gameObject.name == "Bunny 0")
        { damage = 1f; }
        else if (self.gameObject.name == "Bunny 1")
        { damage = 1f; }
        else if (self.gameObject.name == "Bunny 2")
        { damage = 1f; }
        else if (self.gameObject.name == "Bunny 3")
        { damage = 1f; }
        else if (self.gameObject.name == "Slime 0")
        { damage = 1.5f; }
        else if (self.gameObject.name == "Slime 1")
        { damage = 1.5f; }
        else if (self.gameObject.name == "Slime 2")
        { damage = 1.5f; }
        else if (self.gameObject.name == "Slime 3")
        { damage = 1.5f; }
        else if (self.gameObject.name == "LilGhost 0")
        { damage = 2f; }
        else if (self.gameObject.name == "LilGhost 1")
        { damage = 2f; }
        else if (self.gameObject.name == "LilGhost 2")
        { damage = 2f; }
        else if (self.gameObject.name == "LilGhost 3")
        { damage = 2f; }
        else if (self.gameObject.name == "Batty 0")
        { damage = 2.5f; }
        else if (self.gameObject.name == "Batty 1")
        { damage = 2.5f; }
        else if (self.gameObject.name == "Batty 2")
        { damage = 2.5f; }
        else if (self.gameObject.name == "Batty 3")
        { damage = 2.5f; }
        else if (self.gameObject.name == "Mushy")
        { damage = 3f; }
        else if (self.gameObject.name == "BigGhost 0")
        { damage = 3.5f; }
        else if (self.gameObject.name == "BigGhost 1")
        { damage = 3.5f; }
        else if (self.gameObject.name == "BigGhost 2")
        { damage = 3.5f; }
        else if (self.gameObject.name == "BigGhost 3")
        { damage = 3.5f; }
        else if (self.gameObject.name == "BigGhost 4")
        { damage = 3.5f; }
        else if (self.gameObject.name == "BigGhost 5")
        { damage = 3.5f; }
        else if (self.gameObject.name == "BigGhost 6")
        { damage = 3.5f; }
        else if (self.gameObject.name == "BigGhost 7")
        { damage = 3.5f; }
        else if (self.gameObject.name == "BigGhost 8")
        { damage = 3.5f; }
        else if (self.gameObject.name == "Watcher 0")
        { damage = 5f; }
        else if (self.gameObject.name == "Watcher 1")
        { damage = 5f; }
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-20.0f, 20.0f), 0, Random.Range(-20.0f, 20.0f));
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case MonsterState.DEFAULT:
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = MonsterState.CHASE;
                }
                else
                {
                    state = MonsterState.MOVING;
                    destination = transform.position + RandomPosition();
                    agent.SetDestination(destination);
                }
                break;
            case MonsterState.MOVING:
                animator.SetBool("isMoving", true);
                //Debug.Log("Dest = " + destination + "Distance = " + Vector3.Distance(transform.position, destination));
                if (Vector3.Distance(transform.position, destination) < 2.0f)
                {
                    state = MonsterState.DEFAULT;
                }

                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = MonsterState.CHASE;
                }
                break;
            case MonsterState.CHASE:
                if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance)
                {
                    state = MonsterState.DEFAULT;
                }
                agent.SetDestination(player.transform.position);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", true);
                if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
                {
                    state = MonsterState.ATTACK;
                }
                break;
            case MonsterState.ATTACK:
                animator.SetBool("isAttacking", true);
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", false);
                if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance + 1)
                {
                    state = MonsterState.MOVING;
                    animator.SetBool("isAttacking", false);
                }
                break;
            case MonsterState.DEAD:
                animator.SetBool("isDead", true);
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                break;
            default:
                break;
        }
    }

    public void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            bool potion = player.GetComponent<Potions>().potionEffect;
            float damageDealt = self.GetComponent<Monsters>().damage;


            playerControls.takeDamage(damageDealt, potion);
        }
        else if (col.gameObject.CompareTag("Weapon"))
        {
            // Disable all Renderers and Colliders
            //Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
            //foreach (Renderer c in allRenderers) c.enabled = false;
            Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider c in allColliders) c.enabled = false;

            state = MonsterState.DEAD;
            gameObject.GetComponent<ParticleSystemRenderer>().enabled = true;

            StartCoroutine(PlayAndDestroy(3.0f));
            GameStats.UpdateMonstersKilled();
        }
    }

    private IEnumerator PlayAndDestroy(float waitTime)
    {
        myaudio.Play();
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

}

