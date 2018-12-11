using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    //Lanzar una búsqueda en anchura en varios niveles hasta la visualización indicada en EnemyStats
    //En esa búsqueda, buscar el nodo con mayor peso
    public Color colorReachable;
    public Color pathColor;

    public bool move;

    Grid grid;

    List<Nodo> vecinos;

    List<Nodo> allVisible;

    int levelIndex = 0;

    Nodo actual;

    Queue<Nodo> cola;

    int index;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GameManager").GetComponent<Grid>();
        allVisible = new List<Nodo>();
        index = 0;

        actual = grid.NodeFromWorldPoint(transform.position);
        vecinos = grid.GetNeighbours(actual);

        List<Nodo> aux = new List<Nodo>();
        List<Nodo> aux2 = new List<Nodo>();

        cola = new Queue<Nodo>();

        //aux = VisitingNeighbours(actual);

        EnemyVisibility();

       

        
    }

    void EnemyVisibility()
    {

        if(allVisible.Count > 0)
        {
            //Reiniciamos
            //Debug.Log("Hay que reiniciar los visible");
            ResetAllVisibles();
        }
        
        allVisible.Clear();
        actual = grid.NodeFromWorldPoint(transform.position);
        actual.prefab.GetComponent<Renderer>().material.color = pathColor;

        VisitingNeighbours(actual);

        

        cola.Clear();
        
        
    }

    Nodo DecisionMovement()
    {
        Nodo maxValue = allVisible[0];
        foreach(Nodo n in allVisible)
        {
            if(n.resourceCost != -1)
            {
                if(n.resourceCost > maxValue.resourceCost)
                {
                    maxValue = n;
                }
            }
        }
        if(maxValue == allVisible[0] && EnemyExplorerMovement.nodosForVisiting.Count>0)
        {
            //Encontramos el nodo más cercano de los que ha visto el explorador y ese será
            maxValue = GetNodoExplorerCercano();
        }
        return maxValue;

    }

    Nodo GetNodoExplorerCercano()
    {
        
        List<Nodo> aux = EnemyExplorerMovement.nodosForVisiting;
        Nodo cercano = aux[0];
        foreach (Nodo n in aux)
        {
            if(aux.Count == 1)
            {
                return cercano;
            }
            else
            {
                if(actual.GetDistance(n) < actual.GetDistance(cercano))
                {
                    cercano = n;
                }
            }
        }

        return cercano;
    }

    void MoveTo(Nodo n)
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, n.worldPosition, EnemyStats.speed * Time.deltaTime);
        if(this.gameObject.transform.position == n.worldPosition)
        {
            if (n.objectInNode)
            {
                n.objectInNode.GetComponent<Influence>().DestroyThis();
            }
        }
        
    }

    void ResetAllVisibles()
    {
        
        foreach(Nodo n in allVisible)
        {
            n.prefab.GetComponent<Renderer>().material.color = Color.grey;
            n.isVisibleEnemy = true;
            //Debug.Log("es " + n.isVisibleEnemy);
            n.prefab.GetComponent<Renderer>().enabled = true;
        }
    } 

    List<Nodo> VisitingNeighbours(Nodo n)
    {
        //List<Nodo> aux = grid.GetNeighbours(n);
        List<Nodo> aux = grid.GetNeighboursInLevel(n, EnemyStats.cantidadMovimientos);


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
                aux[i].isVisibleEnemy = true;
                cola.Enqueue(aux[i]);
               
            }
        }
        
        return aux;
    }

    // Update is called once per frame
    void Update()
    {

        //VisitingNeighbours(actual);
        //EnemyVisibility();
        if (move)
        {
            MoveTo(DecisionMovement());

        }



    }
}
