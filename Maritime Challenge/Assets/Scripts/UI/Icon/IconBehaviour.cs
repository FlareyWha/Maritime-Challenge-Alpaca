using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconBehaviour : MonoBehaviour
{
    private Camera mapCamera;

    // Start is called before the first frame update
    void Start()
    {
        mapCamera = GameObject.Find("Map Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(mapCamera.orthographicSize * 0.01f, mapCamera.orthographicSize * 0.01f, mapCamera.orthographicSize * 0.01f);
    }
}
