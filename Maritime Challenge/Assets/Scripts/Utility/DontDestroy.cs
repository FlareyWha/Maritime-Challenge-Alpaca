using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //Ensures only one of this object will exist
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");

        if (objs.Length > 1)
        {
            //for (int i = 0; i < objs.Length; ++i)
            //{
            //    if (objs[i].name == gameObject.name)
            //    {
                    Destroy(this.gameObject);
            //        return;
            //    }
            //}
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
