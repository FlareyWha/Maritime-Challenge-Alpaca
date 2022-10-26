using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorial : Tutorial
{
    public override void CheckConditionChanges()
    {
        if (UIManager.Instance.Joystick.IsHeld)
            IncreaseCondition();
    }
}
