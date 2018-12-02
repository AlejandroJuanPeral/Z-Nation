using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform target;
    Transform seeker;
    public Grid grid;
    int indexTarget;


    void Awake()
    {
        //grid = GetComponent<Grid>();
        seeker = this.transform;
        //indexTarget = 0;
    }

    void Update()
    {
        if(target != null)
        {
            FindPath(seeker.position, target.position);

        }
    }
    public void SetTargetNode(Transform t)
    {
        target = t;
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Nodo startNode = grid.NodeFromWorldPoint(startPos);
        Nodo targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Nodo> openSet = new List<Nodo>();
        HashSet<Nodo> closedSet = new HashSet<Nodo>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Nodo node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Nodo neighbour in grid.GetNeighbours(node))
            {
                if (!neighbour.IsWall || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    public List<Nodo> pathPublic;



    void RetracePath(Nodo startNode, Nodo endNode)
    {
        List<Nodo> path = new List<Nodo>();
        Nodo currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        pathPublic = path;

        grid.path = path;

    }

    int GetDistance(Nodo nodeA, Nodo nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
