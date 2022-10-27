using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProjectileManager : NetworkBehaviour
{
    #region Singleton
    public static ProjectileManager Instance = null;
    #endregion

    private int MaxStartingNumProjectiles = 30;

    [SerializeField]
    private GameObject CannonBallPrefab;

    private List<CannonBall> cannonBallsList = new List<CannonBall>();



    public override void OnStartServer()
    {
        for (int i = 0; i < MaxStartingNumProjectiles; i++)
        {
            CannonBall ball = Instantiate(CannonBallPrefab).GetComponent<CannonBall>();
            NetworkServer.Spawn(ball.gameObject);
            ball.gameObject.SetActive(false);
            cannonBallsList.Add(ball);
        }
    }
    private void Awake()
    {
        Instance = this;
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
