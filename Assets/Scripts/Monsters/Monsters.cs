
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum MonsterState { ATTACK, CHASE, MOVING, DEAD, DEFAULT };

[RequireComponent(typeof(NavMeshAgent))]

public class Monsters : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    public float chaseDistance = 10.0f;
    public float damage = 0f;

    protected MonsterState state = MonsterState.DEFAULT;
    protected Vector3 destination = new Vector3(0, 0, 0);

    Animator animator;
    AudioSource myaudio;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        myaudio = GetComponent<AudioSource>();
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
                animator.SetBool("isWalking", false);
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
                animator.SetBool("isWalking", true);
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
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
                if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
                {
                    state = MonsterState.ATTACK;
                }
                break;
            case MonsterState.ATTACK:
                animator.SetBool("isAttacking", true);
                animator.SetBool("isWalking", false);
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

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Weapon"))
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

