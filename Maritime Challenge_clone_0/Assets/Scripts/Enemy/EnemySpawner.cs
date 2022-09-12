using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviourSingleton<EnemySpawner>
{
    [SerializeField]
    private GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector2(9999, 9999), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
