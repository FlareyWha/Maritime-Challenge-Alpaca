using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedemptionItemManager : MonoBehaviour
{
    [SerializeField]
    private Transform redemptionItemListRect;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetRedemptionItems()
    {
        StartCoroutine(DoGetRedemptionItems());
    }

    IEnumerator DoGetRedemptionItems()
    {
        yield return null;
    }
}
