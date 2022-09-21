using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(Camera.main.transform.forward);
    }
}
