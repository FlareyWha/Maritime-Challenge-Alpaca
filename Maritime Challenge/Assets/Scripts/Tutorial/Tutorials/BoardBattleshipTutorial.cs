using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBattleshipTutorial : Tutorial
{
    [SerializeField]
    private Dock dock;

    protected override void Start()
    {
        base.Start();
        conditionText = "Board your battleship at the dock.";
    }

    public override void CheckConditionChanges()
    {
        if (dock.Interacted)
            IncreaseCondition();
    }
}
