using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
 
    [SerializeField]
    private Text PlayerDisplayName;



    public void SetDisplayName(string name)
    {
        PlayerDisplayName.text = name;
    }
}
