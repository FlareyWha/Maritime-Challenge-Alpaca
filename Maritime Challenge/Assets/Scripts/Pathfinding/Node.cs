using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int xPos;
    public int yPos;
    public Node Parent = null;

    public int G = 0; //Cost of path from start node to a given node on the grid
    public int H = 0; //Estimated cost to move from given node on the grid to final destination
    public int F = 0; //Sum of G & H. The smaller the better

    public bool traversable = true;

    public Node(int xPos, int yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public Node(Vector3 position)
    {
        xPos = (int)position.x;
        yPos = (int)position.y;
    }

    public void CalculateFCost()
    {
        F = G + H;
    }
}
