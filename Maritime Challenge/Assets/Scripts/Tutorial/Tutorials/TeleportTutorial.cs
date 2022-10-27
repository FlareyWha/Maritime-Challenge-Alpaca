using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTutorial : Tutorial
{
    protected override void Start()
    {
        base.Start();
        conditionText = "Teleport to the next city using the map.";
    }

    public override void CheckConditionChanges()
    {
        if (TeleportManager.Instance.Teleported)
            IncreaseCondition();

        Debug.Log(TeleportManager.Instance.Teleported);
    }
}
