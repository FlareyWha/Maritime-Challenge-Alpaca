using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviourSingleton<AStarPathfinding>
{
    public const int STRAIGHT_MOVEMENT_COST = 10;
    public const int DIAGONAL_MOVEMENT_COST = 14; //14 cus A^2 + B^2 = C^2 where A=B=1

    private Grid grid;

    //[SerializeField]
    //private int gridWidth, gridHeight;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3> FindPath(Vector3Int gridLowerLimits, Vector3 startPos, Vector3 endPos, int gridWidth, int gridHeight)
    {
        Node[,] allNodes = new Node[gridWidth, gridHeight];
        List<Node> openList; //List of nodes to check
        HashSet<Node> closedList; //List of nodes checked

        Node currentNode, startNode, endNode;

        //Add new nodes for the start and end
        startNode = new Node(grid.WorldToCell(startPos));
        endNode = new Node(grid.WorldToCell(endPos));

        Debug.Log("Start node pos: " + startNode.xPos + ", " + startNode.yPos);
        Debug.Log("End node pos: " + endNode.xPos + ", " + endNode.yPos);

        //Loop through the given area to create nodes and store them
        for (int i = 0; i < allNodes.GetLength(0); ++i)
        {
            for (int j = 0; j < allNodes.GetLength(1); j++)
            {
                allNodes[i, j] = new Node(gridLowerLimits.x + i, gridLowerLimits.y + j);
                allNodes[i, j].G = int.MaxValue;
                allNodes[i, j].CalculateFCost();
            }    
        }

        startNode.G = 0;
        startNode.H = CalculateHCost(startNode, endNode);
        startNode.CalculateFCost();

        //Add start node into the open list
        openList = new List<Node> { startNode };
        closedList = new HashSet<Node>();

        while (openList.Count > 0)
        {
            //Get the node with the lowest F cost
            currentNode = GetLowestFCostNode(openList);
            
            //Return the path to check if the pos of the current node is same as endNode
            if (currentNode.xPos == endNode.xPos && currentNode.yPos == endNode.yPos)
                return CalculatePath(currentNode);

            //Remove node from checking list and add it to checked list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //Gets the list of available neighbours to check
            List<Node> availableNeighbours = GetAvailableNeighbours(allNodes, gridLowerLimits, currentNode, gridWidth, gridHeight);

            //Loops through all the available neighbours
            foreach (Node neighbourNode in availableNeighbours)
            {
                //Skips this neighbour if closed list contains it, as it has already been checked
                if (closedList.Contains(neighbourNode))
                    continue;

                int currentToNeighbourGCost = currentNode.G + CalculateHCost(currentNode, neighbourNode);

                //Debug.LogWarning("Neighbour cost: " + neighbourNode.G);
                //Debug.LogWarning("currentToNeighbourGCost cost: " + currentToNeighbourGCost);
                //Debug.LogWarning("F cost: " + neighbourNode.F);

                //Checks if the cost is lower than the current neighbours cost
                if (currentToNeighbourGCost < neighbourNode.G)
                {
                    neighbourNode.Parent = currentNode;
                    neighbourNode.G = currentToNeighbourGCost;
                    neighbourNode.H = CalculateHCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                        openList.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    Node GetNode(Node[,] allNodes, Vector3Int gridLowerLimits, int nodeXPos, int nodeYPos)
    {
        //Debug.Log("XNode: " + (nodeXPos - gridLowerLimits.x));
        //Debug.Log("YNode: " + (nodeYPos - gridLowerLimits.y));
        Node node = allNodes[nodeXPos - gridLowerLimits.x, nodeYPos - gridLowerLimits.y];
        return node;
    }

    int CalculateHCost(Node firstNode, Node secondNode)
    {
        int xDistance = Mathf.Abs(firstNode.xPos - secondNode.xPos);
        int yDistance = Mathf.Abs(firstNode.yPos - secondNode.yPos);
        int remainder = Mathf.Abs(xDistance - yDistance);

        //Returns the amount you are able to move diagonally and the amount you are able to move straight
        return DIAGONAL_MOVEMENT_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_MOVEMENT_COST * remainder;
    }

    Node GetLowestFCostNode(List<Node> openList)
    {
        Node lowestCostNode = openList[0];

        for (int i = 1; i < openList.Count; ++i)
        {
            if (openList[i].F < lowestCostNode.F)
                lowestCostNode = openList[i];
        }

        return lowestCostNode;
    }

    List<Node> GetAvailableNeighbours(Node[,] allNodes, Vector3Int gridLowerLimits, Node currentNode, int gridWidth, int gridHeight)
    {
        List<Node> availableNeighbourList = new List<Node>();

        //Check left
        if (currentNode.xPos - 1 >= gridLowerLimits.x)
        {
            availableNeighbourList.Add(GetNode(allNodes, gridLowerLimits, currentNode.xPos - 1, currentNode.yPos));

            //Check bottom left
            if (currentNode.yPos - 1 >= gridLowerLimits.y)
                availableNeighbourList.Add(GetNode(allNodes, gridLowerLimits, currentNode.xPos - 1, currentNode.yPos - 1));

            //Check top left
            if (currentNode.yPos + 1 < gridLowerLimits.y + gridHeight)
                availableNeighbourList.Add(GetNode(allNodes, gridLowerLimits, currentNode.xPos - 1, currentNode.yPos + 1));
        }
        //Check right
        if (currentNode.xPos + 1 < gridLowerLimits.x + gridWidth)
        {
            availableNeighbourList.Add(GetNode(allNodes, gridLowerLimits, currentNode.xPos + 1, currentNode.yPos));

            //Check bottom right
            if (currentNode.yPos - 1 >= gridLowerLimits.y)
                availableNeighbourList.Add(GetNode(allNodes, gridLowerLimits, currentNode.xPos + 1, currentNode.yPos - 1));

            //Check top right
            if (currentNode.yPos + 1 < gridLowerLimits.y + gridHeight)
                availableNeighbourList.Add(GetNode(allNodes, gridLowerLimits, currentNode.xPos + 1, currentNode.yPos + 1));
        }
        //Check bottom
        if (currentNode.yPos - 1 >= gridLowerLimits.y)
            availableNeighbourList.Add(GetNode(allNodes, gridLowerLimits, currentNode.xPos, currentNode.yPos - 1));
        //Check top
        if (currentNode.yPos + 1 < gridLowerLimits.y + gridHeight)
            availableNeighbourList.Add(GetNode(allNodes, gridLowerLimits, currentNode.xPos, currentNode.yPos + 1));

        return availableNeighbourList;
    }

    private List<Vector3> CalculatePath(Node endNode)
    {
        List<Vector3> path = new List<Vector3> { grid.CellToWorld(new Vector3Int(endNode.xPos, endNode.yPos, 0)) };
        Node currentNode = endNode;

        //Loops through the parents until it reaches back to the start
        while (currentNode.Parent != null)
        {
            path.Add(grid.CellToWorld(new Vector3Int(currentNode.Parent.xPos, currentNode.Parent.yPos, 0)));
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        return path;
    }
}
