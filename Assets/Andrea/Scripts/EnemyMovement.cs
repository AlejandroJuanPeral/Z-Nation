using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    //Lanzar una búsqueda en anchura en varios niveles hasta la visualización indicada en EnemyStats
    //En esa búsqueda, buscar el nodo con mayor peso

    Grid grid;

    List<Nodo> vecinos;

    List<Nodo> allVisible;

    int levelIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GameManager").GetComponent<Grid>();
        allVisible = new List<Nodo>();

        Nodo actual = grid.NodeFromWorldPoint(transform.position);
        vecinos = grid.GetNeighbours(actual);


        for (int i = 0; i < vecinos.Count; i++)
        {
            allVisible.Add(vecinos[i]);
            VisitingNeighbours(vecinos[i]);

        }
        levelIndex++; //Nivel de anchura

       /* while(aux <= EnemyStats.cantidadMovimientos)
        {
            for (int i = 0; i < vecinos.Count; i++)
            {

                vecinos[i].prefab.GetComponent<Renderer>().material.color = Color.green;

            }
            actual = vecinos[vecinos.Count - 1];
            aux++;
            vecinos = grid.GetNeighbours(actual);
        }*/
        
    }

    List<Nodo> VisitingNeighbours(Nodo n)
    {
        List<Nodo> aux = grid.GetNeighbours(n);
        for (int i = 0; i < aux.Count; i++)
        {
            if(aux[i].prefab.GetComponent<Renderer>().material.color == Color.green)
            {
                continue;
            }
            else
            {
                aux[i].prefab.GetComponent<Renderer>().material.color = Color.green;
                allVisible.Add(aux[i]);
            }
        }
        return aux;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
