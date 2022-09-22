using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BaseAbandonedCity : MonoBehaviour
{
    [SerializeField]  //Honestly idk i need to find a way to do this better but else uhhhhhhhh
    protected int abandonedCityID;

    protected List<BaseEnemy> enemyList = new List<BaseEnemy>();
    protected List<Player> playerList = new List<Player>();

    [SerializeField]
    protected int abandonedCityAreaCellWidth, abandonedCityAreaCellHeight;
    protected Vector2 abandonedCityAreaLowerLimit, abandonedCityAreaUpperLimit;
    protected Vector3Int gridMovementAreaLowerLimit, gridMovementAreaUpperLimit;

    protected bool cleared = false;
    protected int capturedGuildID = 0;
    [SerializeField]
    protected Text capturedGuildName;

    protected void OnDrawGizmos()
    {
        //Draw something to visualize the box area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(abandonedCityAreaUpperLimit.x - abandonedCityAreaLowerLimit.x, abandonedCityAreaUpperLimit.y - abandonedCityAreaLowerLimit.y, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        //Check whether abandoned city with this id has been cleared or not
        StartCoroutine(CheckClearedGuildID());

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

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

        boxCollider2D.size = new Vector2(abandonedCityAreaCellWidth, abandonedCityAreaCellHeight);
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
            capturedGuildID = enemyKiller.GetGuildID();
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

    IEnumerator CheckClearedGuildID()
    {
        string url = ServerDataManager.URL_getAbandonedCityCapturedGuildID;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iAbandonedCityID", abandonedCityID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                int iCapturedGuildID = int.Parse(webreq.downloadHandler.text);
                if (iCapturedGuildID != -1)
                {
                    cleared = true;
                    capturedGuildID = iCapturedGuildID;
                }
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
