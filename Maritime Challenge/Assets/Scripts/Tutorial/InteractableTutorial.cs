using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTutorial : Tutorial
{
    [SerializeField]
    private TutorialSign tutorialSign;

    protected override void Start()
    {
        base.Start();
        conditionText = "Interact with the sign.";
    }

    public override void CheckConditionChanges()
    {
        if (tutorialSign.Interacted)
            IncreaseCondition();
    }
}
