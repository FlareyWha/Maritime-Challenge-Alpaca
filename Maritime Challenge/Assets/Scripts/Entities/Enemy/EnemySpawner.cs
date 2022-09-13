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


    void Start()
    {
        Instance = this;

      
        if (isServer)
        {
            Spawn();
        }
    }

    void Update()
    {
        
    }
  
    [Server]
    private void Spawn()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector2(9999, 9999), Quaternion.identity);
        NetworkServer.Spawn(newEnemy);
    }

    [Command]
    public void AskServerToSpawn()
    {
        Spawn();
    }
}
