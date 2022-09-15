using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Server : Mirror.Examples.MultipleAdditiveScenes.MultiSceneNetManager
{
    public override void Start()
    {
        StartServer();
    }

}
