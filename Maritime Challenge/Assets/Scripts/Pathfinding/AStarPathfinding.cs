using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public const int STRAIGHT_MOVEMENT_COST = 10;
    public const int DIAGONAL_MOVEMENT_COST = 14;

    private List<Node> allNodes;
    private List<Node> openList; //List of nodes to check
    private HashSet<Node> closedList; //List of nodes checked

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private int gridWidth, gridHeight;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Node> FindPath(Grid grid, Vector3 startPos, Vector3 endPos)
    {
        Node currentNode, startNode, endNode;

        //Add new nodes for the start and end
        startNode = new Node(grid.WorldToCell(startPos));
        endNode = new Node(grid.WorldToCell(endPos));

        for (int i = 0; i < gridWidth; ++i)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                Node node = new Node(i, j);
                node.G = int.MaxValue;
                node.CalculateFCost();

                allNodes.Add(node);
            }    
        }

        //Add start node into the open list
        openList = new List<Node> { startNode };
        closedList = new HashSet<Node>();

        startNode.G = 0;
        startNode.H = CalculateHCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            //Get the node with the lowest F cost
            currentNode = GetLowestFCostNode();
            
            //Return the path to check
            if (currentNode == endNode)
                return CalculatePath(endNode);

            //Remove node from checking list and add it to checked list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //Gets the list of available neighbours to check
            List<Node> availableNeighbours = GetAvailableNeighbours(currentNode);

            //Loops through all the available neighbours
            foreach (Node neighbourNode in availableNeighbours)
            {
                //Skips this neighbour if closed list contains it, as it has already been checked
                if (closedList.Contains(neighbourNode))
                    continue;

                int currentToNeighbourGCost = currentNode.G + CalculateHCost(currentNode, neighbourNode);

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

    int CalculateHCost(Node firstNode, Node secondNode)
    {
        int xDistance = Mathf.Abs(firstNode.xPos - secondNode.xPos);
        int yDistance = Mathf.Abs(firstNode.yPos - secondNode.yPos);
        int remainder = Mathf.Abs(xDistance - yDistance);

        //Returns the amount you are able to move diagonally and the amount you are able to move straight
        return DIAGONAL_MOVEMENT_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_MOVEMENT_COST * remainder;
    }

    Node GetLowestFCostNode()
    {
        Node lowestCostNode = openList[0];

        for (int i = 1; i < openList.Count; ++i)
        {
            if (openList[i].F < lowestCostNode.F)
                lowestCostNode = openList[i];
        }

        return lowestCostNode;
    }

    List<Node> GetAvailableNeighbours(Node currentNode)
    {
        List<Node> availableNeighbourList = new List<Node>();

        //Check left
        if (currentNode.xPos - 1 >= 0)
        {
            availableNeighbourList.Add(new Node(currentNode.xPos - 1, currentNode.yPos));

            //Check bottom left
            if (currentNode.yPos - 1 >= 0)
                availableNeighbourList.Add(new Node(currentNode.xPos - 1, currentNode.yPos - 1));

            //Check top left
            if (currentNode.yPos + 1 < gridHeight)
                availableNeighbourList.Add(new Node(currentNode.xPos - 1, currentNode.yPos + 1));
        }
        //Check right
        if (currentNode.xPos + 1 < gridWidth)
        {
            availableNeighbourList.Add(new Node(currentNode.xPos + 1, currentNode.yPos));

            //Check bottom right
            if (currentNode.yPos - 1 >= 0)
                availableNeighbourList.Add(new Node(currentNode.xPos + 1, currentNode.yPos - 1));

            //Check top right
            if (currentNode.yPos + 1 < gridHeight)
                availableNeighbourList.Add(new Node(currentNode.xPos + 1, currentNode.yPos + 1));
        }
        //Check bottom
        if (currentNode.yPos - 1 >= 0)
            availableNeighbourList.Add(new Node(currentNode.xPos, currentNode.yPos - 1));
        //Check top
        if (currentNode.yPos + 1 < gridHeight)
            availableNeighbourList.Add(new Node(currentNode.xPos, currentNode.yPos + 1));

        return availableNeighbourList;
    }

    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node> { endNode };
        Node currentNode = endNode;

        //Loops through the parents until it reaches back to the start
        while (currentNode.Parent != null)
        {
            path.Add(currentNode.Parent);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        return path;
    }
}
