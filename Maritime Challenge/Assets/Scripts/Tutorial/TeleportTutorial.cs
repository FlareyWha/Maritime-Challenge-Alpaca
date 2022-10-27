using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTutorial : Tutorial
{
    public override void CheckConditionChanges()
    {
        if (TeleportManager.Instance.Teleported)
            IncreaseCondition();
    }
}
