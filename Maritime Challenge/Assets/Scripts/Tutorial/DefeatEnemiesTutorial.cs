using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemiesTutorial : Tutorial
{
    private List<BaseEnemy> enemies = new List<BaseEnemy>();

    protected override void Start()
    {
        base.Start();
        conditionText = "Defeat all the enemies.";
    }

    public override void InitTutorial()
    {
        enemies.AddRange(GameObject.FindObjectsOfType<BaseEnemy>());
        maxCondition = enemies.Count;
    }

    public override void CheckConditionChanges()
    {
        int indexToRemove = -1;

        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] == null)
            {
                IncreaseCondition();
                indexToRemove = i;
            }
        }

        if (indexToRemove != -1)
            enemies.Remove(enemies[indexToRemove]);
    }
}
