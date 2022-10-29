using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockTutorial : Tutorial
{
    [SerializeField]
    private Dock dock;

    protected override void Start()
    {
        base.Start();
        conditionText = "Sail to the city on the right to dock.";
    }

    public override void CheckConditionChanges()
    {
        if (dock.Interacted)
            IncreaseCondition();
    }
}
