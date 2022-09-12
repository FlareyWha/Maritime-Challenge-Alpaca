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

        GameObject newEnemy = Instantiate(enemyPrefab, new Vector2(9999, 9999), Quaternion.identity);

        //if (isServer)
        //{
        //    Spawn();
        //    Debug.Log("SERVER RUNS START");
        //}
    }

    void Update()
    {
        
    }
  
    private void Spawn()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector2(9999, 9999), Quaternion.identity);
        NetworkServer.Spawn(newEnemy);

    }
}
