using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_STATES
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK,
    NO_ENEMY_STATE
}

public class BaseEnemy : BaseEntity
{
    protected Vector2 spawnPoint;
    protected float distanceToSpawnPoint;
    [SerializeField]
    protected float maxDistanceToSpawnPoint = 30f;

    protected Player currentTargetPlayer;
    protected Vector2 directionToPlayer;
    protected float distanceToPlayer;
    [SerializeField]
    protected float detectionDistance = 10f;

    [SerializeField]
    protected float maxIdleTime = 5f;

    [SerializeField]
    protected float maxPatrolTime = 10f;

    [SerializeField]
    protected float maxSpotTime = 1f;

    [SerializeField]
    protected float maxChaseTime = 5f;

    private float timer;
    private float maxTimer;
    protected ENEMY_STATES currEnemyState;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        FindPlayerToTarget();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleFSM();
    }

    protected virtual void HandleFSM()
    {
        //Find distance to player if a player target is active
        if (currentTargetPlayer != null)
            distanceToPlayer = Vector2.Distance(currentTargetPlayer.transform.position, transform.position);

        switch (currEnemyState)
        {
            case ENEMY_STATES.IDLE:
                HandleIdle();
                break;
            case ENEMY_STATES.PATROL:
                HandlePatrol();
                break;
            case ENEMY_STATES.CHASE:
                HandleChase();
                break;
            case ENEMY_STATES.ATTACK:
                HandleAttack();
                break;
        }

        timer += Time.deltaTime;
    }

    protected virtual void HandleIdle()
    {
        if (timer >= maxTimer)
        {
            currEnemyState = ENEMY_STATES.PATROL;
            ResetTimer(maxPatrolTime);

            Debug.Log("Enemy patrolling");
        }
        else if (distanceToPlayer <= detectionDistance)
        {
            currEnemyState = ENEMY_STATES.CHASE;
            ResetTimer(maxChaseTime);

            Debug.Log("Enemy spotted player");
        }
    }

    protected virtual void HandlePatrol()
    {

        if (timer >= maxTimer)
        {
            currEnemyState = ENEMY_STATES.IDLE;
            ResetTimer(maxIdleTime);

            Debug.Log("Enemy resting");
        }
        else if (distanceToPlayer <= detectionDistance)
        {
            currEnemyState = ENEMY_STATES.CHASE;
            ResetTimer(maxChaseTime);

            Debug.Log("Enemy spotted player");
        }
    }

    protected virtual void HandleChase()
    {
        distanceToSpawnPoint = Vector3.Distance(spawnPoint, transform.position);

        if (timer >= maxTimer || distanceToSpawnPoint >= maxDistanceToSpawnPoint || distanceToPlayer > detectionDistance)
        {
            currEnemyState = ENEMY_STATES.IDLE;
            ResetTimer(maxIdleTime);
        }

        Move();
    }

    protected virtual void HandleAttack()
    {

    }

    protected void FindPlayerToTarget()
    {
        //Find a player in range somehow i suppose
        //currentTargetPlayer = 
    }

    protected void GetDirectionToPlayer()
    {
        directionToPlayer = currentTargetPlayer.transform.position - transform.position;
    }

    protected void ResetTimer(float newMaxTimer)
    {
        timer = 0;
        maxTimer = newMaxTimer;
    }

    protected void Move()
    {
        transform.Translate(directionToPlayer * movespd * Time.deltaTime);
    }
}
