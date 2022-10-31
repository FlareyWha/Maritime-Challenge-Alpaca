using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorial : Tutorial
{
    private float timer;

    protected override void Start()
    {
        base.Start();
        conditionText = "Move the joystick.";
    }

    public override void CheckConditionChanges()
    {
        if (UIManager.Instance.Joystick.IsHeld)
        {
            timer += Time.deltaTime;

            if (timer >= 1)
                IncreaseCondition();
        }
        else
            timer = 0;
    }
}
