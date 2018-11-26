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




    public Nodo(bool a_IsWall, Vector3 _worldPos, int _gridX, int _gridY)
    {
        IsWall = a_IsWall;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
