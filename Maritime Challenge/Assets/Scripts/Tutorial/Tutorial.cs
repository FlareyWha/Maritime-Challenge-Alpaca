using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial
{
    private int condition;

    private int maxCondition;
    public int MaxCondition
    { 
        get { return maxCondition; }
        set { maxCondition = value; }
    }

    public void IncreaseCondition(int value = 1)
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
