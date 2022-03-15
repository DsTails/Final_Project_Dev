using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyAIStates
{
    idle,
    patrol,
    wait,
    strafe,
    chase,
    attack
}

public class AdvancedEnemyAI : MonoBehaviour
{
    enemyAIStates defaultState;
    [Header("Enemy State")]
    public enemyAIStates state;

    [Header("Radius - Attack and Detect")]
    public float detectRadius;
    public float attackRadius;
    public float meleeAttackRadius;

    [Header("Speed Values")]
    public float attackSpeed;
    public float moveSpeed;
    public float patrolSpeed;
    public float strafeSpeed;

    [Header("Attack Rates")]
    public float rangeAttackTimer;
    public float meleeAttackTimer;
    float rangeAttackTime, meleeAttackTime;

    [Header("Strafe Values")]
    public float strafeTimer;
    float strafeTime;
    [SerializeField]
    bool canStrafe;
    Transform strafePosition;
    [SerializeField]
    int strafeDirectionMultiplier;

    Transform playerTarget;

    [Header("Patrol Values")]
    public Transform[] patrolPoints;
    public float minPatrolDistance;

    [Header("Enemy Stats")]
    public float enemyMaxHealth;
    public float enemyHealth;
    public bool hasArmour;
    public float ArmourDurability;
    float defaultArmDurability;

    [Header("Wait Timers")]
    public float waitTimer;
    public float patrolWaitTimer;
    float waitTime, patrolWaitTime;
    int patrolIndex = 1;

    public GameObject bullet;
    public Transform enemyFirepoint;
    

    bool hasResetRot;

    public Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        defaultState = state;
        playerTarget = PlayerBase.instance.transform;
        enemyHealth = enemyMaxHealth;
        rangeAttackTime = rangeAttackTimer;
        meleeAttackTime = meleeAttackTimer;
        strafeTime = strafeTimer;
        waitTime = waitTimer;
        patrolWaitTime = patrolWaitTimer;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePause.paused)
        {
            return;
        }

        
        distanceChecks();
        

        //possibility have the timer count down either in the wait state or the strafe state, so the enemy acts out of the strafe instead of just waiting.
        if(state == enemyAIStates.wait)
        {
            if(waitTime > 0)
            {
                waitTime -= GamePause.deltaTime;
            }
            else
            {
                makeDecision();
            }
        }

        if(state == enemyAIStates.strafe)
        {
            if(strafeTime > 0)
            {
                enemyStrafe();
                strafeTime -= 1 * GamePause.deltaTime;
            }
            else
            {
                state = enemyAIStates.wait;
                waitTime = waitTimer;
                strafeTime = strafeTimer;
            }
        } else if(state == enemyAIStates.patrol)
        {
            patrol();
        } else if(state == enemyAIStates.attack)
        {
            attack();
        }
    }

    public void distanceChecks()
    {
        //if player in range then chase...
        //in normal context
        //For advanced AI, have the enemy turn to face the player
        //then have decisions made at random
        if(Vector3.Distance(transform.position, playerTarget.position) <= detectRadius)
        {
            
            //For now, have enemy immediately respond to player's presence
            //Can always tweak to respond to sounds/attacks later
            if (Vector3.Distance(transform.position, playerTarget.position) > attackRadius)
            {
                if (state != enemyAIStates.wait)
                {
                    state = enemyAIStates.wait;
                    hasResetRot = false;
                }
            }
            else
            {
                state = enemyAIStates.attack;
            }
        }
        else
        {
            state = defaultState;
            if(!hasResetRot && state == enemyAIStates.patrol)
            {
                transform.LookAt(patrolPoints[patrolIndex]);
                hasResetRot = true;
            }
        }
    }

    //Will be used to determine AI's next action
    public void makeDecision()
    {
        int randomNum = 0;
        if(randomNum == 0)
        {
            
            strafePosition = playerTarget;
            var randomVal = Random.Range(0.50f, 1.00f);
            state = enemyAIStates.strafe;
            Debug.Log("strafe");
            if(randomVal >= .75f)
            {
                strafeDirectionMultiplier = 1;
            }
            else
            {
                strafeDirectionMultiplier = -1;
            }
        }
        else
        {
            approachPlayer();
        }
    }

    public void approachPlayer()
    {

    }

    public void attack()
    {
        if(Vector3.Distance(transform.position, playerTarget.position) < meleeAttackRadius)
        {
            //Melee Attack
        }
        else
        {
            if(rangeAttackTime <= 0)
            {
                //Shoot
                //For now, the AI will still have the basic shoot action. But in future, will have more than one option for ranged combat
                Debug.Log("SHOOT");
                rangeAttack();
            }
            else
            {
                rangeAttackTime -= 1 * GamePause.deltaTime;
            }
        }
    }
    //This may possibly be divided into different range attacks
    public void rangeAttack()
    {
        GameObject newBullet = Instantiate(bullet, enemyFirepoint.position, enemyFirepoint.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = transform.forward * 10;
        rangeAttackTime = rangeAttackTimer;
    }

    public void meleeAttack()
    {

    }

    public void enemyStrafe()
    {
        transform.LookAt(strafePosition);
        transform.RotateAround(strafePosition.position, new Vector3(0, 1, 0), strafeSpeed * strafeDirectionMultiplier * GamePause.deltaTime);
    }

    public void patrol()
    {
        if(patrolWaitTime <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[patrolIndex].position, patrolSpeed * GamePause.deltaTime);

            if(Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) < minPatrolDistance)
            {
                patrolIndex++;

                if(patrolIndex > patrolPoints.Length - 1)
                {
                    patrolIndex = 0;
                }

                transform.LookAt(patrolPoints[patrolIndex]);
                patrolWaitTime = patrolWaitTimer;
            }
        }
        else
        {
            patrolWaitTime -= 1 * GamePause.deltaTime;
        }
    }


    public void TakeDamage(float DamageTaken, bool concentratedShot)
    {
        /*if (check for armour is true THEN) { 
         * 
         * if(ArmourDur > 0){
         *      damage the armour
         * } else{
         *      damage the enemy
         * }
         * 
         * }*/

        if (concentratedShot)
        {
            if (hasArmour)
            {
                //If it has armour, check if they have armour left
                if(ArmourDurability > 0)
                {
                    ArmourDurability -= DamageTaken;
                }
                else
                {
                    //Determine whether or not it is ineffective against non-armoured enemies or enemies who have lost their armour
                }
            }
            else
            {

            }
        }
        else
        {
            if (hasArmour)
            {
                if(ArmourDurability > 0)
                {
                    //Make it impossible for the player to damage armoured enemies with normal weapons
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
