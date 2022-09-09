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
    [SerializeField]
    protected Vector2 spawnPoint;
    protected float distanceToSpawnPoint;
    [SerializeField]
    protected int movementAreaCellWidth, movementAreaCellHeight;
    protected Vector2 movementAreaLowerLimit, movementAreaUpperLimit;

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

    protected float timer;
    protected float maxTimer;
    protected ENEMY_STATES currEnemyState;

    protected float aStarTimer;
    protected List<Vector3Int> path;
    protected Vector3 destination;
    protected Vector3 currentMovementDirection;
    protected bool firstMove;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        transform.position = spawnPoint;

        Grid grid = GameObject.Find("Grid").GetComponent<Grid>();

        Vector3Int spawnPointGrid = grid.WorldToCell(spawnPoint);

        //Set the limits for where the enemy can go
        movementAreaLowerLimit = grid.CellToWorld(new Vector3Int(spawnPointGrid.x - movementAreaCellWidth / 2, spawnPointGrid.y - movementAreaCellHeight / 2, 0));
        movementAreaUpperLimit = grid.CellToWorld(new Vector3Int(spawnPointGrid.x + movementAreaCellWidth / 2, spawnPointGrid.y + movementAreaCellHeight / 2, 0));

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

        if (timer >= maxTimer || CheckOutOfBounds() || distanceToPlayer > detectionDistance)
        {
            currEnemyState = ENEMY_STATES.IDLE;
            ResetTimer(maxIdleTime);
        }

        Move();
    }

    protected virtual void HandleAttack()
    {
        //Choose an attack and play its animation or something, once done go into cooldown or go back to chase i suppose
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

    protected void CheckAStar()
    {
        if (aStarTimer < 0)
        {
            //Get path

            aStarTimer = 1.25f;
            firstMove = true;

            foreach (Vector3Int position in path)
            {
                if (firstMove)
                {
                    destination = position;
                    currentMovementDirection = destination - transform.position;
                    firstMove = false;
                }
                else
                {
                    if (position - destination == currentMovementDirection)
                    {
                        destination = position;
                    }
                    else
                        break;
                }
            }
        }

        aStarTimer -= Time.deltaTime;
    }

    protected void Move()
    {
        CheckAStar();

        transform.Translate(currentMovementDirection * movespd * Time.deltaTime);
    }


    protected bool CheckOutOfBounds()
    {
        if (transform.position.x < movementAreaLowerLimit.x || transform.position.x > movementAreaUpperLimit.x || transform.position.y < movementAreaLowerLimit.y || transform.position.y > movementAreaUpperLimit.y)
        {
            return true;
        }

        return false;
    }
}
