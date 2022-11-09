using System;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviourSingleton<VFXManager>
{
    [SerializeField]
    private List<VFX_TYPE> VFXIndex;
    [SerializeField]
    private List<GameObject> VFXPrefabs;

    private Dictionary<VFX_TYPE, GameObject> VFXList = new Dictionary<VFX_TYPE, GameObject>();

    protected override void Awake()
    {
        base.Awake();

        if (VFXIndex.Count != VFXPrefabs.Count)
        {
            Debug.LogError("VFXManager: VFX Prefabs Count does not match VFX Index Count! Please Double Check!:(((((");
            return;
        }

        for (int i = 0; i < VFXIndex.Count; i++)
        {
            VFXList.Add(VFXIndex[i], VFXPrefabs[i]);
        }
    }

    public void AddVFX(VFX_TYPE type, Vector3 position)
    {
        Instantiate(VFXList[type], position, Quaternion.identity);
    }
}

public enum VFX_TYPE
{
    CANNONBALL_HIT,

    NUM_TOTAL
}
