using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tutorial : MonoBehaviour
{
    protected int condition = 0;

    protected int maxCondition;
    public int MaxCondition
    { 
        get { return maxCondition; }
        set { maxCondition = value; }
    }

    protected string objectiveText;
    public string ObjectiveText
    {
        get { return objectiveText; }
        private set { }
    }
    protected string conditionText;
    public string ConditionText
    {
        get { return conditionText; }
        private set { }
    }


    protected virtual void Start()
    {
        objectiveText = "Do the tutorial.";
        conditionText = "AAAAAAAAAAAAAAAAAAAA";
        maxCondition = 1;
    }

    public virtual void InitTutorial()
    {

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
