using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tutorial : MonoBehaviour
{
    protected int condition = 0;
    protected int maxCondition;
    protected string objectiveText;
    protected string conditionText;

    protected TutorialHUDManager tutorialHUDManager;


    protected virtual void Start()
    {
        objectiveText = "Do the tutorial.";
        conditionText = "AAAAAAAAAAAAAAAAAAAA";
        maxCondition = 1;
    }

    public virtual void InitTutorial(TutorialHUDManager tutorialHUDManager)
    {
        this.tutorialHUDManager = tutorialHUDManager;
        this.tutorialHUDManager.UpdateTutorialHUD(objectiveText, conditionText, condition, maxCondition);
    }

    public virtual void CheckConditionChanges()
    {

    }

    protected void IncreaseCondition(int value = 1)
    {
        condition += value;
    }

    public bool CheckConditionCleared()
    {
        if (condition >= maxCondition)
            return true;

        return false;
    }
}
