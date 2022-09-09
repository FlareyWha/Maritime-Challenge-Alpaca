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
    protected Rigidbody2D rb;

    [SerializeField]
    protected Vector2 spawnPoint;
    protected float distanceToSpawnPoint;
    [SerializeField]
    protected int movementAreaCellWidth, movementAreaCellHeight;
    protected Vector2 movementAreaLowerLimit, movementAreaUpperLimit;
    protected Vector3Int gridMovementAreaLowerLimit, gridMovementAreaUpperLimit;

    protected Player currentTargetPlayer;
    protected Vector2 directionToPlayer;
    protected float distanceToPlayer = int.MaxValue;
    [SerializeField]
    protected float detectionDistance = 10f;
    [SerializeField]
    protected float findPlayerDistance = 1000f;

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
    protected ENEMY_STATES currEnemyState = ENEMY_STATES.IDLE;

    protected float aStarTimer;
    protected List<Vector3> path = new List<Vector3>();
    protected Vector3 destination;
    protected Vector2 currentMovementDirection;
    protected int pathIncrement;

    [SerializeField]
    protected LayerMask playerLayerMask;

    private void OnDrawGizmos()
    {
        //Draw something to visualize the box area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnPoint, new Vector3(movementAreaUpperLimit.x - movementAreaLowerLimit.x, movementAreaUpperLimit.y - movementAreaLowerLimit.y, 0));
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Grid grid = GameObject.Find("Grid").GetComponent<Grid>();

        //Make sure spawn point will start from the edge of the grid
        spawnPoint.x -= spawnPoint.x % grid.cellSize.x;
        spawnPoint.y -= spawnPoint.y % grid.cellSize.y;

        //Set current position to spawn point
        transform.position = spawnPoint;

        Vector3Int gridSpawnPoint = grid.WorldToCell(spawnPoint);

        //Make sure cell width and height will always fit cell size
        movementAreaCellWidth -= Mathf.RoundToInt(movementAreaCellWidth % grid.cellSize.x);
        movementAreaCellHeight -= Mathf.RoundToInt(movementAreaCellHeight % grid.cellSize.y);

        //Save this for later for a*
        gridMovementAreaLowerLimit = new Vector3Int(gridSpawnPoint.x - movementAreaCellWidth / 2, gridSpawnPoint.y - movementAreaCellHeight / 2, 0);
        gridMovementAreaUpperLimit = new Vector3Int(gridSpawnPoint.x + movementAreaCellWidth / 2, gridSpawnPoint.y + movementAreaCellHeight / 2, 0);

        //Set the limits for where the enemy can go
        movementAreaLowerLimit = grid.CellToWorld(gridMovementAreaLowerLimit);
        movementAreaUpperLimit = grid.CellToWorld(gridMovementAreaUpperLimit);

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
        else
            FindPlayerToTarget();

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

        //Collider[] players = Physics.OverlapSphere(transform.position, findPlayerDistance);

        //foreach (Collider player in players)
        //{
        //    if (player.gameObject.CompareTag("Player"))
        //    {
        //        currentTargetPlayer = player.GetComponent<Player>();
        //    }
        //}

        GameObject[] playersToChooseFrom = GameObject.FindGameObjectsWithTag("Player");

        if (playersToChooseFrom.Length > 0)
            currentTargetPlayer = playersToChooseFrom[Random.Range(0, playersToChooseFrom.Length)].GetComponent<Player>();
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
        if (currentTargetPlayer == null)
            return;

        if (aStarTimer < 0)
        {
            Debug.Log(currentTargetPlayer);
            Debug.Log(AStarPathfinding.Instance);

            //Get path
            path = AStarPathfinding.Instance.FindPath(gridMovementAreaLowerLimit, transform.position, currentTargetPlayer.transform.position, movementAreaCellWidth, movementAreaCellHeight);

            aStarTimer = 1.25f;

            pathIncrement = 0;
            destination = path[pathIncrement];
            currentMovementDirection = destination - transform.position;
            pathIncrement++;
        }

        aStarTimer -= Time.deltaTime;
    }

    protected void CheckMovementDirection()
    {
        if (Vector2.Distance(transform.position, destination) < 0.5f)
        {
            if (pathIncrement < path.Count)
            {
                destination = path[pathIncrement];
                currentMovementDirection = (destination - transform.position);
                pathIncrement++;
            }
            else
            {
                Debug.LogWarning("Reached end of path");
            }
        }
    }

    protected void Move()
    {
        CheckAStar();

        CheckMovementDirection();

        rb.position += currentMovementDirection * movespd * Time.deltaTime;
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
