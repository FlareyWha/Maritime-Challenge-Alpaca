using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
        else
            Destroy(this);
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
