using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Color colorReachable;
    public Color pathColor;

    Grid grid;

    List<Nodo> allVisible;
    Nodo actual;


    public bool isMoving = false;

    public int cantNodos = PlayerStats.cantidadNodosPorTurno;
    

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GameManager").GetComponent<Grid>();

        allVisible = new List<Nodo>();

        actual = grid.NodeFromWorldPoint(transform.position);
        actual.prefab.GetComponent<Renderer>().material.color = pathColor;
        VisitingNeighbours(actual);

    }

    void ResetAllVisibles()
    {
        foreach ( Nodo n in allVisible)
        {
            n.prefab.GetComponent<Renderer>().material.color = Color.grey;
            n.isVisiblePlayer = true;
            n.prefab.GetComponent<Renderer>().enabled = true;
            
        }
        allVisible.Clear();

    }

    List<Nodo> VisitingNeighbours (Nodo n)
    {
        List<Nodo> aux = grid.GetNeighboursInLevel(n, PlayerStats.cantidadNodosPorTurno);

        for (int i = 0; i < aux.Count; i++)
        {
            if(aux[i].prefab.GetComponent<Renderer>().material.color == colorReachable)
            {
                continue;
            }
            else
            {
                aux[i].prefab.GetComponent<Renderer>().material.color = colorReachable;
                allVisible.Add(aux[i]);
                aux[i].isVisiblePlayer = true;
                
            }
        }
        return aux;
    }

    public void NextTurn()
    {
        cantNodos = PlayerStats.cantidadNodosPorTurno;
    }

    void PlayerVisibility()
    {
        if(allVisible.Count> 0)
        {
            ResetAllVisibles();
        }
        actual = grid.NodeFromWorldPoint(transform.position);
        actual.prefab.GetComponent<Renderer>().material.color = pathColor;

        VisitingNeighbours(actual);
    }

    public void MoveTo(Nodo n)
    {
        Nodo anteriorAux = grid.NodeFromWorldPoint(transform.position);

        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, n.worldPosition, EnemyStats.speed * Time.deltaTime);

        Nodo actualAux = grid.NodeFromWorldPoint(transform.position);

        if (actualAux != anteriorAux)
        {
            cantNodos--;
            if (cantNodos == 0)
            {
                isMoving = false;
                Debug.Log("Se ha terminado el turno ");
            }
        }

        if (this.gameObject.transform.position == n.worldPosition)
        {
            if (n.objectInNode)
            {
                n.objectInNode.GetComponent<Influence>().DestroyThis();
            }
        }
        PlayerVisibility();


    }

    // Update is called once per frame
    void Update()
    {
        PlayerVisibility();
    }
}
