using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProjectileManager : MonoBehaviourSingleton<ProjectileManager>
{

    private int MaxStartingNumProjectiles = 30;

    [SerializeField]
    private GameObject CannonBallPrefab;

    private List<CannonBall> cannonBallsList = new List<CannonBall>();


    [Server]
    private void Start()
    {

        if (!GameHandler.Instance.isServer)
            return;

        for (int i = 0; i < MaxStartingNumProjectiles; i++)
        {
            CannonBall ball = Instantiate(CannonBallPrefab).GetComponent<CannonBall>();
            NetworkServer.Spawn(ball.gameObject);
            ball.gameObject.SetActive(false);
            cannonBallsList.Add(ball);
        }
    }

    public CannonBall GetActiveCannonBall()
    {
        foreach (CannonBall ball in cannonBallsList)
        {
            if (!ball.active)
                return ball;
        }

        SpawnMoreCannonBalls(10);
        return GetActiveCannonBall();
    }

    private void SpawnMoreCannonBalls(int num)
    {
        for (int i = 0; i < num; i++)
        {
            CannonBall ball = Instantiate(CannonBallPrefab).GetComponent<CannonBall>();
            NetworkServer.Spawn(ball.gameObject);
            cannonBallsList.Add(ball);
        }
    }



}
