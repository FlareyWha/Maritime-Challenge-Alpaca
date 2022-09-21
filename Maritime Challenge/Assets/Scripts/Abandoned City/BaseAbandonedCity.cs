using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAbandonedCity : MonoBehaviour
{
    protected List<BaseEnemy> enemyList = new List<BaseEnemy>();
    protected List<Player> playerList = new List<Player>();

    [SerializeField]
    protected int abandonedCityAreaCellWidth, abandonedCityAreaCellHeight;
    protected Vector2 abandonedCityAreaLowerLimit, abandonedCityAreaUpperLimit;
    protected Vector3Int gridMovementAreaLowerLimit, gridMovementAreaUpperLimit;

    protected bool cleared = false;
    protected int clearedGuildID = 0;
    [SerializeField]
    protected Text clearedGuildName;

    protected void OnDrawGizmos()
    {
        //Draw something to visualize the box area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(abandonedCityAreaUpperLimit.x - abandonedCityAreaLowerLimit.x, abandonedCityAreaUpperLimit.y - abandonedCityAreaLowerLimit.y, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        Grid grid = GameObject.Find("Grid").GetComponent<Grid>();

        //Make sure cell width and height will always fit cell size
        abandonedCityAreaCellWidth -= Mathf.RoundToInt(abandonedCityAreaCellWidth % grid.cellSize.x);
        abandonedCityAreaCellHeight -= Mathf.RoundToInt(abandonedCityAreaCellHeight % grid.cellSize.y);

        //Can delete these late this is just to see whether the sizes work
        Vector3Int gridSpawnPoint = grid.WorldToCell(transform.position);
        gridMovementAreaLowerLimit = new Vector3Int(gridSpawnPoint.x - abandonedCityAreaCellWidth / 2, gridSpawnPoint.y - abandonedCityAreaCellHeight / 2, 0);
        gridMovementAreaUpperLimit = new Vector3Int(gridSpawnPoint.x + abandonedCityAreaCellWidth / 2, gridSpawnPoint.y + abandonedCityAreaCellHeight / 2, 0);
        abandonedCityAreaLowerLimit = grid.CellToWorld(gridMovementAreaLowerLimit);
        abandonedCityAreaUpperLimit = grid.CellToWorld(gridMovementAreaUpperLimit);
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void OnFirstPlayerEnterArea()
    {
        SpawnEnemies();
    }

    public void OnLastPlayerLeaveArea()
    {
        ResetAbandonedCity();
    }

    public void AddToPlayerList(Player player)
    {
        playerList.Add(player);

        if (playerList.Count == 1)
            OnFirstPlayerEnterArea();
    }

    public void RemoveFromPlayerList(Player player)
    {
        playerList.Remove(player);

        if (playerList.Count == 0)
            OnLastPlayerLeaveArea();
    }

    public void AddToEnemyList(BaseEnemy enemy)
    {
        enemyList.Add(enemy);
    }

    public void RemoveFromEnemyList(BaseEnemy enemy, Player enemyKiller)
    {
        enemyList.Remove(enemy);
        CheckAreaCleared(enemyKiller);
    }

    public void CheckAreaCleared(Player enemyKiller)
    {
        if (enemyList.Count == 0)
        {
            cleared = true;
            clearedGuildID = enemyKiller.GetGuildID();
            //clearedGuildName.text = ???

            //Set info in database
        }
    }

    public void SpawnEnemies()
    {
        //Spawn enemies and make sure to set the abandoned city variable in the enemies to this one.
    }

    public void ResetAbandonedCity()
    {
        enemyList.Clear();
        playerList.Clear();
    }
}
