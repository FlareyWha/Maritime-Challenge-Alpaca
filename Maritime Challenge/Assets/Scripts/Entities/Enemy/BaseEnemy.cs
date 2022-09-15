using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

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
    protected Transform targetPosition;
    public Transform TargetPosition
    {
        get { return targetPosition; }
        private set { }
    }

    [SerializeField]
    protected Vector2 spawnPoint;
    [SerializeField]
    protected int movementAreaCellWidth, movementAreaCellHeight;
    protected Vector2 movementAreaLowerLimit, movementAreaUpperLimit;
    protected Vector3Int gridMovementAreaLowerLimit, gridMovementAreaUpperLimit;

    protected Battleship currentTargetPlayer;
    protected float distanceToPlayer = int.MaxValue;
    [SerializeField]
    protected float detectionDistance = 10f;
    [SerializeField]
    protected float findPlayerDistance = 20f;

    [SerializeField]
    protected float maxIdleTime = 5f;
    [SerializeField]
    protected float maxPatrolTime = 10f;
    [SerializeField]
    protected float maxSpotTime = 1f;
    [SerializeField]
    protected float maxChaseTime = 5f;
    [SerializeField]
    protected float attackDistance = 2f;

    [SerializeField]
    private Image HPFill;

    protected float timer;
    protected float maxTimer;
    protected ENEMY_STATES currEnemyState = ENEMY_STATES.IDLE;

    protected float aStarTimer;
    protected List<Vector3> path = new List<Vector3>();
    protected Vector3 destination;
    protected Vector2 currentMovementDirection;
    protected int pathIncrement;

    protected void OnDrawGizmos()
    {
        //Draw something to visualize the box area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnPoint, new Vector3(movementAreaUpperLimit.x - movementAreaLowerLimit.x, movementAreaUpperLimit.y - movementAreaLowerLimit.y, 0));

        if (path.Count > 2)
        {
            for (int i = 0; i < path.Count - 1; ++i)
            { 
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.InitSpriteSize();
        rb = GetComponent<Rigidbody2D>();

        if (!isServer)
            return;

        Debug.Log("Enemy Start Called");

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
        base.CheckForEntityClick();

        if (!isServer)
            return;

        HandleFSM();
    }

    protected virtual void HandleFSM()
    {
        //Find distance to player if a player target is active
        if (currentTargetPlayer != null)
        {
            distanceToPlayer = Vector2.Distance(currentTargetPlayer.transform.position, transform.position);

            //Try to find player again if player exceeds the findPlayerDistance
            if (distanceToPlayer > findPlayerDistance)
            {
                distanceToPlayer = int.MaxValue;
                currentTargetPlayer = null;
                FindPlayerToTarget();
            }
        }
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
            GetPatrolDestination();
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

        PatrolMovement();
    }

    protected virtual void HandleChase()
    {
        if (timer >= maxTimer || CheckOutOfBounds() || distanceToPlayer > detectionDistance)
        {
            currEnemyState = ENEMY_STATES.IDLE;
            ResetTimer(maxIdleTime);
        }
        else if (distanceToPlayer < attackDistance)
        {
            currEnemyState = ENEMY_STATES.ATTACK;
        }

        ChaseMovement();
    }

    protected virtual void HandleAttack()
    {
        //Choose an attack and play its animation or something, once done go into cooldown or go back to chase i suppose
        currentTargetPlayer.GetOwner().TakeDamage(10);

        currEnemyState = ENEMY_STATES.IDLE;
        ResetTimer(maxIdleTime);
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
            currentTargetPlayer = playersToChooseFrom[Random.Range(0, playersToChooseFrom.Length)].GetComponent<Battleship>();
    }

    protected void ResetTimer(float newMaxTimer)
    {
        timer = 0;
        maxTimer = newMaxTimer;
    }

    protected void FindAStarPath(Vector3Int endGridPos)
    {
        path = AStarPathfinding.Instance.FindPath(gridMovementAreaLowerLimit, transform.position, endGridPos, movementAreaCellWidth, movementAreaCellHeight);

        SetStartOfPath();
    }

    protected void FindAStarPath(Vector3 endPos)
    {
        path = AStarPathfinding.Instance.FindPath(gridMovementAreaLowerLimit, transform.position, endPos, movementAreaCellWidth, movementAreaCellHeight);

        SetStartOfPath();
    }

    protected void SetStartOfPath()
    {
        pathIncrement = 0;
        destination = path[pathIncrement];
        currentMovementDirection = destination - transform.position;
        pathIncrement++;
    }

    protected void PatrolMovement()
    {
        CheckMovementDirection(true);

        Move();
    }

    protected void GetPatrolDestination()
    {
        Vector3Int randPatrolDestination = new Vector3Int(gridMovementAreaLowerLimit.x + Random.Range(0, movementAreaCellWidth), gridMovementAreaLowerLimit.y + Random.Range(0, movementAreaCellHeight), 0);

        FindAStarPath(randPatrolDestination);
    }

    protected void ChaseMovement()
    {
        CheckAStar();

        CheckMovementDirection(false);

        Move();
    }

    protected void CheckAStar()
    {
        if (currentTargetPlayer == null)
            return;

        if (aStarTimer < 0)
        {
            FindAStarPath(currentTargetPlayer.transform.position);

            aStarTimer = 1.25f;
        }

        aStarTimer -= Time.deltaTime;
    }

    protected void CheckMovementDirection(bool patrolling)
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
                //Debug.LogWarning("Reached end of path");
                currentMovementDirection = Vector2.zero;

                if (patrolling)
                {
                    GetPatrolDestination();
                }
            }
        }
    }

    protected void Move()
    {
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
    protected override void OnHPChanged(int oldHP, int newHP)
    {
        Debug.Log("PLAYER HP CHANGED");
        HPFill.fillAmount = (float)newHP / maxHp;
    }
    public override void OnEntityClicked()
    {
        PlayerData.MyPlayer.GetBattleShip().SetTarget(this);
    }
}