using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorial : Tutorial
{
    protected override void Start()
    {
        base.Start();
        conditionText = "Move the joystick.";
    }

    public override void CheckConditionChanges()
    {
        if (UIManager.Instance.Joystick.IsHeld)
            IncreaseCondition();
    }
}
