using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enemyStates
{
    idle,
    patrol,
    chase,
    attack
}
public class EnemyAI : MonoBehaviour
{
    public enemyStates state;
    enemyStates defaultState;
    public float chaseRadius;
    public float attackRadius;

    public float moveSpeed;
    public float rateOfAttack;
    float attackTimer;

    [SerializeField]
    Transform target;

    [SerializeField]
    Transform[] patrolPoints;
    public float minimumCheckpointRadius;
    int patrolIndex = 1;

    

    Vector3 originalPosition;
    void Start()
    {
        defaultState = state;
        target = PlayerBase.instance.transform;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePause.paused)
        {
            return;
        }

        //Distance Checks

        distanceChecks();
        
        
        if(state == enemyStates.chase || state == enemyStates.attack)
        {
            //Chase the player (Separate Method)
            chaseTarget();
        } else if(state == enemyStates.idle)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * GamePause.deltaTime);
        } else if(state == enemyStates.patrol)
        {
            patrol();
        }

        if(attackTimer > 0)
        {
            attackTimer -= 1 * GamePause.deltaTime;
        }
    }

    public void chaseTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * GamePause.deltaTime);

        if (Vector3.Distance(target.position, transform.position) < attackRadius)
        {
            state = enemyStates.attack;
        }

        if (state == enemyStates.attack)
        {
            if (attackTimer <= 0)
            {
                Debug.Log("ATTACK");
                attackTimer = rateOfAttack;
            }
        }
    }

    void distanceChecks()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius)
        {
            //Change to chase state here
            state = enemyStates.chase;
        }
        else
        {
            //return to idle/patrol
            state = defaultState;
            //if idle, return to default position

        }
    }

    void patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[patrolIndex].position, moveSpeed * GamePause.deltaTime);
        if(Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) <= minimumCheckpointRadius)
        {
            //Move on to the next patrol point
            patrolIndex++;
            if(patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
