using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour
{
    public bool gizmoActivate;

    public Color colorNormal;
    public Color colorUnwalkable;

    private Renderer renderNodePrefab;

    public GameObject prefabNode;

    public LayerMask unwalkableMask; //Walls and obstacles
    public Vector2 gridWorldSize; //Size for the grid
    public float nodeRadius; //size of the node
    Nodo[,] grid; //Matrix of nodes for the grid 

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        renderNodePrefab = prefabNode.GetComponent<Renderer>();


        nodeDiameter = nodeRadius * 2;
        prefabNode.gameObject.transform.localScale = new Vector3(nodeDiameter - 0.1f , 0.5f, nodeDiameter - 0.1f);
        //Dependiendo del tamaño de los nodos y el tamaño del grid en el mundo, crearemos el tamaño del grid
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Nodo[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Nodo(walkable, worldPoint, x, y);
                GameObject aux = Instantiate(prefabNode, worldPoint, Quaternion.identity);
                
                grid[x, y].prefab = aux;
                

                renderNodePrefab = aux.GetComponent<Renderer>();
                if(walkable == false)
                {
                    renderNodePrefab.material.color = colorUnwalkable;
                }
                else
                {
                    renderNodePrefab.material.color = colorNormal;
                }

                renderNodePrefab.enabled = false;
            }
        }
    }

    public Nodo[,] GetGrid()
    {
        return grid;
    }


    public List<Nodo> GetNeighbours(Nodo node)
    {
        List<Nodo> neighbours = new List<Nodo>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public List<Nodo> GetNeighboursInLevel(Nodo node, int level)
    {
        List<Nodo> neighbours = new List<Nodo>();

        for (int x = (-1)* level; x <= level; x++)
        {
            for (int y = (-1) * level; y <= level; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Nodo NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Nodo> path;


    //Auxiliar function for see all the nodes in the grid
    void OnDrawGizmos()
    {
        if (gizmoActivate)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null)
            {
                foreach (Nodo n in grid)
                {

                    if (n.IsWall)
                    {
                        Gizmos.color = Color.white;
                    }
                    else
                    {
                        Gizmos.color = Color.yellow;
                    }
                    //Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    if (path != null)
                        if (path.Contains(n))
                            Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }

    }
    private void OnMouseDown()
    {
        Debug.Log("ULSADAF");
    }

    public List<Nodo> GetPath()
    {
        return path;
    }
}
