using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tutorial
{
    private int condition;

    private int maxCondition;
    public int MaxCondition
    { 
        get { return maxCondition; }
        set { maxCondition = value; }
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
