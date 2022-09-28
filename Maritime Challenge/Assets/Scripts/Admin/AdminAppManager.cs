using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminAppManager : MonoBehaviour
{
    [SerializeField]
    private GameObject registerPanel, redemptionPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRegisterPanelButtonClicked()
    {
        redemptionPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void OnRedemptionPanelButtonClicked()
    {
        registerPanel.SetActive(false);
        redemptionPanel.SetActive(true);
    }
}
