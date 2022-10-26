using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockTutorial : Tutorial
{
    private Dock dock;

    public override void InitTutorial()
    {
        dock = GameObject.Find("Dock").GetComponent<Dock>();
    }

    public override void CheckConditionChanges()
    {
        if (dock.Interacted)
            IncreaseCondition();
    }
}
