using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    protected bool active = true;

    public virtual void Update()
    {

    }

    public bool IsActive()
    {
        return active;
    }
}