using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_STATES
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK,
    NO_ENEMY_STATE
}

public class BaseEnemy : MonoBehaviour
{
    protected BaseEntity baseEntity;
    protected ENEMY_STATES currEnemyState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleFSM();
    }

    protected virtual void HandleFSM()
    {
        switch (currEnemyState)
        {
            case ENEMY_STATES.IDLE:
                break;
            case ENEMY_STATES.PATROL:
                break;
            case ENEMY_STATES.CHASE:
                break;
            case ENEMY_STATES.ATTACK:
                break;
        }
    }
}
