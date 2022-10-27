using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHUDManager : MonoBehaviour
{
    [SerializeField]
    private Text objectiveText, conditionText, conditionAmountText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTutorialHUD(string objective, string condition, int conditionAmount, int conditionMaxAmount)
    {
        objectiveText.text = objective;
        conditionText.text = condition;
        UpdateConditionAmountText(conditionAmount, conditionMaxAmount);
    }

    public void UpdateConditionAmountText(int conditionAmount, int conditionMaxAmount)
    {
        if (conditionMaxAmount <= 1)
            conditionAmountText.text = "";
        else
            conditionAmountText.text = conditionAmount + "/" + conditionMaxAmount;
    }
}
