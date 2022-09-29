using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Mirror;

public class BaseAbandonedCity : NetworkBehaviour
{
    [SerializeField]  //Honestly idk i need to find a way to do this better but else uhhhhhhhh
    protected int abandonedCityID;

    protected List<BaseEnemy> allEnemies = new List<BaseEnemy>();
    protected List<BaseEnemy> enemyList = new List<BaseEnemy>();
    protected List<Player> playerList = new List<Player>();

    [SerializeField]
    protected int abandonedCityAreaCellWidth, abandonedCityAreaCellHeight;
    protected Vector2 abandonedCityAreaLowerLimit, abandonedCityAreaUpperLimit;
    protected Vector3Int gridMovementAreaLowerLimit, gridMovementAreaUpperLimit;

    protected bool captured = false;
    protected int capturedGuildID = 0;
    [SerializeField]
    protected Text capturedGuildName;

    protected Grid grid;

    protected void OnDrawGizmos()
    {
        //Draw something to visualize the box area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(abandonedCityAreaUpperLimit.x - abandonedCityAreaLowerLimit.x, abandonedCityAreaUpperLimit.y - abandonedCityAreaLowerLimit.y, 0));
    }

    IEnumerator GetGrid()
    {
        while (GridManager.Instance == null)
        {
            yield return null;
        }

        grid = GridManager.Instance.Grid;

        ResizeColliderSize(grid);
    }

    private void Start()
    {
        StartCoroutine(GetGrid());
    }

    [Server]
    public void InitAbandonedCity(int id, int areaCellWidth, int areaCellHeight, Vector2 position, int guildID)
    {
        abandonedCityID = id;
        abandonedCityAreaCellWidth = areaCellWidth;
        abandonedCityAreaCellHeight = areaCellHeight;
        transform.position = position;
        capturedGuildID = guildID;

        if (capturedGuildID != -1)
        {
            captured = true;
        }
      
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

        InitEnemies();
    }

    [Server]
    public void InitEnemies()
    {
        for (int i = 0; i < 5; ++i)
        {
            GameObject baseEnemyGameObject = Instantiate(EnemyAssetManager.Instance.BlobTheFish, transform.position + new Vector3(0, 0, 0), Quaternion.identity);

            NetworkServer.Spawn(baseEnemyGameObject);

            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(baseEnemyGameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName("WorldHubScene"));

            baseEnemyGameObject.SetActive(false);

            allEnemies.Add(baseEnemyGameObject.GetComponent<BaseEnemy>());
        }
    }
    
    public void ResizeColliderSize(Grid grid)
    {
        //Set the size for the collider
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(abandonedCityAreaCellWidth * grid.cellSize.x, abandonedCityAreaCellHeight * grid.cellSize.y);

        Debug.Log(boxCollider2D.size);
    }

    public void OnFirstPlayerEnterArea()
    {
        Debug.Log("Player In Range - Spawning Enemies..");
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
            CaptureAbandonedCity(enemyKiller);

            //Set info in database
            StartCoroutine(UpdateClearedGuildID());
        }
    }

    public void CaptureAbandonedCity(Player enemyKiller)
    {
        captured = true;
        capturedGuildID = enemyKiller.GetGuildID();
        //clearedGuildName.text = ???

        DestroyEnemies();
    }

    public void SpawnEnemies()
    {
        //Spawn enemies and make sure to set the abandoned city variable in the enemies to this one.
        foreach (BaseEnemy enemy in allEnemies)
        {
            enemy.gameObject.SetActive(true);
        }

        enemyList = allEnemies;

        Debug.Log("Spawned enemies");
    }

    public void UnspawnEnemies()
    {
        foreach (BaseEnemy enemy in enemyList)
        {
            enemy.gameObject.SetActive(false);
        }

        enemyList.Clear();
    }

    public void DestroyEnemies()
    {
        enemyList.Clear();

        foreach (BaseEnemy enemy in allEnemies)
        {
            NetworkServer.Destroy(enemy.gameObject);
        }

        allEnemies.Clear();
    }

    public void ResetAbandonedCity()
    {
        //Unspawn all the enemies
        UnspawnEnemies();

        playerList.Clear();

       // Debug.Log("Resetted abandoned city");
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddToPlayerList(collision.GetComponent<Player>());
        }
    }

    [ServerCallback]
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RemoveFromPlayerList(collision.GetComponent<Player>());
        }
    }

    IEnumerator UpdateClearedGuildID()
    {
        string url = ServerDataManager.URL_updateAbandonedCityCapturedGuildID;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iAbandonedCityID", abandonedCityID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError(webreq.downloadHandler.text);
                break;
        }
    }
}
