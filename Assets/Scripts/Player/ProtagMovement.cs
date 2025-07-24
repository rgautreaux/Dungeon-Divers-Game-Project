using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


// THIS SCRIPT DRAWS FROM CODE FROM MY PAST SCHOOL ASSIGNMENTS AND THE EASYSTART THIRD PERSON CONTROLLER SCRIPT IN ORDER TO FIT THE PLAYER MODELS I CHOSE FOR THIS PROJECT
// I do not claim to have created the base structure, and instead am using it as a template/starting-point to work from to create a script that suits my models.
// Credit goes to the creator of the EasyStart Third Person Controller on Unity.


public class ProtagMovement : MonoBehaviour
{
    //Player Health
    public float health = 100.0f;

    //Character Movement Speed
    public float velocity = 5f;

    //Sprint Speed
    public float sprintAdittion = 3.5f;

    //Jump Height
    public float jumpHeight = 1.5f;

    //Jump Duration
    public float jumpTime = 0.85f;

    //Gravity
    public float gravity = 9.8f;

    //Enemy Alertness
    public float monsterDistance = 20.0f;


    // Checks the character's current state
    bool isRunning = false;
    bool isSprinting = false;
    bool isJumping = false;
    bool isAttacking = false;
    bool isDefending = false;
    bool isWeak = false;
    bool isDead = false;

    // Identifiable game keys input for the player
    float inputHorizontal;
    float inputVertical;
    bool inputJump;
    bool inputCrouch;
    bool inputSprint;

    // Get control of the character's animations
    public Animator animator;
    // Gets the character's collision and movement controller component
    public CharacterController cc;

    // Variable controlling the time the player spent in the air. Explained further below.
    float jumpElapsedTime = 0;

    //Detecting monsters
    GameObject monster;


    void Start()
    {
        // Starts any of the above variables when starting the game
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        monster = GameObject.FindWithTag("Monster");
    }


    // Update is only being used here to identify keys and trigger animations
    void Update()
    {
        monster = GameObject.FindWithTag("Monster");

        // Check which input is being pressed
        // Read the end of this script for a detailed explanation.

        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputJump = Input.GetAxis("Jump") == 1f;
        inputSprint = Input.GetAxis("Fire3") == 1f;
        // Unfortunately GetAxis does not work with GetKeyDown, so inputs must be taken individually
        inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);


        // isGrounded is a Character Controller property that informs whether the player is touching the ground. It's very easy to use!
        if (cc.isGrounded)
        {
            // Check the player's speed and whether it is high enough to trigger the running animation
            float minimumSpeed = 0.9f;
            if (cc.velocity.magnitude > minimumSpeed)
            {
                Debug.Log("Player is running!");

                animator.SetBool("isRunning", true);
                animator.SetBool("isIdle", false);
                animator.SetBool("isReady", false);
                animator.SetBool("isWeak", false);


            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", true);
                Debug.Log("Player stopped running");

            }
        }

        // Same logic as the run, but adding the sprint input condition
        if (cc.velocity.magnitude > 0.9f && inputSprint)
        {
            Debug.Log("Player is sprinting");
            isSprinting = true;
            isRunning = false;
        }
        else
        {
            Debug.Log("Player stopped sprinting");
            isSprinting = false;
            isRunning = true;

        }

        // After going through the above condition, we already have the answer to whether it is running or not within the variable
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("sprint", isSprinting);

        // Air/jumping animation if is or not in the ground
        if (cc.isGrounded == true)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", true);

        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", true);
        }

        // Check if input jump is pressed and if player is in the ground
        if (inputJump && cc.isGrounded)
        {
            isJumping = true;
            // Disable crounching when jumping? You decide, just uncomment:
            // isCrouching = false;
        }

        // It's at the end of the code. Leave it for later.
        HeadHittingDetect();


        if (!isRunning && !isJumping && !isSprinting && !isDefending && !isAttacking && !isDead)
        {
            if (Vector3.Distance(transform.position, monster.transform.position) < monsterDistance)
            {
                Debug.Log("Player is ready for a fight!");
                animator.SetBool("isReady", true);
            }
            else if (health <= 50)
            {
                Debug.Log("Player needs healing");
                animator.SetBool("isWeak", true);
            }
            else
            {
                animator.SetBool("isIdle", true);
                Debug.Log("Player is standing Idle!");

            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("isDefending", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isDefending", false);
        }
    }          
                  
    // With the inputs and animations defined, FixedUpdate is responsible for applying movements and actions to the player
    private void FixedUpdate()
    {
        // Basic character movement and jumping
        // Checks if the player is sprinting, because if he is, it will add a speed boost to his movement
        float velocityAdittion = 0;
        if (isSprinting)
        {
            velocityAdittion = sprintAdittion;
        }

        // Let's use the player's inputs to tell us if he moved to either side
        // And if it moved, let's make it faster by multiplying by speed and reducing by Time.DeltaTime
        // More explanations in the end
        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        // The Y position is the upward movement, and as our character doesn't fly, he just stays at zero :)
        float directionY = 0;

        // Let's check if the player jumped
        if (isJumping)
        {
            // We are using a Unity function to make the jump more "subtle" and apply the natural feeling of inertia of the movement
            //  https://docs.unity3d.com/ScriptReference/Mathf.SmoothStep.html

            directionY = Mathf.SmoothStep(jumpHeight, jumpHeight * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;

            // Increases the time that has passed since the player started the jump
            jumpElapsedTime += Time.deltaTime;

            // But if the elapsed time is longer than expected jump time, it's time to start falling
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false; // It's not jumping anymore
                jumpElapsedTime = 0; // We reset the time so that the player can jump again later
            }
        }

        // After calculating the movement and jump, it's time to apply gravity
        // It needs to be negative so that the game always throws the player down
        directionY = directionY - gravity * Time.deltaTime;

        // Apply rotation to player faces in direction of the pressed input

        // locate which side is the player's front and right side.
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // We will not rotate the Y axis, as this would cause the player to hit the ground
        forward.y = 0;
        right.y = 0;

        // Normalization makes the calculation uniform
        // If you don't put this in, your player will move faster if he moves diagonally
        forward.Normalize();
        right.Normalize();

        // Let's relate the front with the Z direction (3d depth) and right with X (lateral movement)
        forward = forward * directionZ;
        right = right * directionX;

        // This condition is used to check whether the player is moving.
        // If you don't add it, the player will always look at the origin of the world after releasing a key. It's even funny, try it.
        if (directionX != 0 || directionZ != 0)
        {
            // This function returns the angle in radians whose tangent is the quotient of the two given arguments
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            // Applies the player's previously calculated rotation.
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            // Slerp makes movement smoother. If you want, you can test like this: transform.rotation = rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        /*
            This is the end of the character's rotation. The code below just applies everything to the character's actual movement 
        */

        // Vertical movement is gravity or jumping. All of this has already been previously defined in the directionY
        Vector3 verticalDirection = Vector3.up * directionY;
        // Here is the conversion of forward and right movement to actual positioning in the world based on X and Z (Y is above)
        Vector3 horizontalDirection = forward + right;

        // Finally, we apply this movement to the character controller, which will move our player
        Vector3 moviment = verticalDirection + horizontalDirection;
        cc.Move(moviment);
    }


    //This function makes the character end his movement if he hits his head on something.
    // If you remove this, you will notice that your character continues to "float" in the air if he hits his head against a wall
    void HeadHittingDetect()
    {
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = cc.height / 2f * headHitDistance;

        // Uncomment this line to see the Ray drawed in your characters head
        // Debug.DrawRay(ccCenter, Vector3.up * headHeight, Color.red);

        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }

    public void takeDamage(float damage, bool strengthPotion)
    {
        //Base Damage
        health -= damage;
        strengthPotion = GetComponent<Potions>().potionEffect;

        //Strength Potion Effect
        if (strengthPotion)
        {

            health -= damage / 2;
        }



        Debug.Log("Monster dealth " + damage + "damage, " + health + "HP remain.");

        if (health <= 0)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isReady", false);
            animator.SetBool("isWeak", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDefending", false);

            animator.SetBool("isDead", true);
        }
    }

    public void UpdateSpeed(float speedPotion)
    {
        //Increase Base Character Movement Speed
        velocity = speedPotion;
    }
}