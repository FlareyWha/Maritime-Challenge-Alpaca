using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemiesTutorial : Tutorial
{

    private List<BaseEnemy> enemies = new List<BaseEnemy>();

    public override void InitTutorial()
    {
        enemies.AddRange(GameObject.FindObjectsOfType<BaseEnemy>());
        MaxCondition = enemies.Count;
    }

    public override void CheckConditionChanges()
    {
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] == null)
            {
                IncreaseCondition();
                enemies.Remove(enemies[i]);
            }
        }
    }
}
