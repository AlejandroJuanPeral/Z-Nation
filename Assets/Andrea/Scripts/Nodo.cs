using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Nodo 
{
   

    public GameObject prefab;

    public bool IsWall;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Nodo parent;

    public int resourceCost;




    public Nodo(bool a_IsWall, Vector3 _worldPos, int _gridX, int _gridY)
    {
        IsWall = a_IsWall;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;


        resourceCost = -1;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    //Para tener la distancia del nodo en el que estamos al nodo que queramos
    public int GetDistance(Nodo nodeB)
    {
        int dstX = Mathf.Abs(this.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(this.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
