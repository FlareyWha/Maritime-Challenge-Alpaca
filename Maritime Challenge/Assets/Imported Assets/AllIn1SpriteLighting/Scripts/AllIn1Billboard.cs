using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AllIn1Billboard : MonoBehaviour
{
    [SerializeField] private bool billboardOnStart = true, billboardEveryFrame, inBothAxis = true;

    [Space, Header("If null Main Camera will be used")]
    [SerializeField] private Camera cam;

    [Space, Header("Orientation")]
    [SerializeField] private bool flipForward;

    private Vector3 lookAtPoint;

    void Start()
    {
        if (billboardOnStart) BillboardMe();
    }

    void Update()
    {
        if (billboardEveryFrame) BillboardMe();
    }

    [ContextMenu("Billboard Transform Towards Camera")]
    void BillboardMe()
    {
        if (cam == null) GetMainCameraReference();

        lookAtPoint = cam.transform.position;

        if (!inBothAxis) lookAtPoint.y = 0f;
        transform.LookAt(lookAtPoint);

        if (flipForward) transform.forward = -transform.forward;
    }

    [ContextMenu("Get Main Camera Reference")]
    void GetMainCameraReference()
    {
        cam = Camera.main;
    }
}