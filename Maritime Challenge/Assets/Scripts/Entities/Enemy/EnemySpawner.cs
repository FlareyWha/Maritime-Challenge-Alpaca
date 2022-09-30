using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpawner : NetworkBehaviour
{
    #region Singleton
    public static EnemySpawner Instance = null;
    #endregion

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject eelBossPrefab;


    void Start()
    {
        Instance = this;

        //NetworkServer.SpawnObjects();
        //Debug.Log("Spawned Network GOs Count: " + NetworkServer.spawned.Count);
        //foreach (KeyValuePair<uint, NetworkIdentity> spawned in NetworkServer.spawned)
        //{
        //    Debug.Log("Spawned: " + spawned.Value.gameObject.name);
        //}

    }

    void Update()
    {
        
    }
  
    [Server]
    private void Spawn(Vector2 spawnPos)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        SceneManager.Instance.MoveGameObjectToScene(newEnemy, "WorldHubScene");
        NetworkServer.Spawn(newEnemy);
        newEnemy.GetComponent<BaseEnemy>().InitEnemy(spawnPos);
    }

    [Command]
    public void AskServerToSpawn(Vector2 spawnPos)
    {
        Spawn(spawnPos);
    }

}
