using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviourSingleton<GridManager>
{
    [SerializeField]
    private Grid grid;
    public Grid Grid
    {
        get { return grid; }
        private set { }
    }
}
