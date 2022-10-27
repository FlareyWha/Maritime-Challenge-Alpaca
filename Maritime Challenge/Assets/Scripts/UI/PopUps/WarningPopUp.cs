using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPopUp : MonoBehaviour
{
    public void OnCloseButtonClicked()
    {
        Destroy(this);
    }
}
