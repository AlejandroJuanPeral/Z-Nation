﻿using System.Collections;
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

    Nodo actual;

    Queue<Nodo> cola;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GameManager").GetComponent<Grid>();
        allVisible = new List<Nodo>();

        actual = grid.NodeFromWorldPoint(transform.position);
        vecinos = grid.GetNeighbours(actual);

        List<Nodo> aux = new List<Nodo>();
        List<Nodo> aux2 = new List<Nodo>();

        cola = new Queue<Nodo>();

        aux = VisitingNeighbours(actual);

        EnemyVisibility();
        
        
    }

    void EnemyVisibility()
    {

        if(allVisible.Count > 0)
        {
            //Reiniciamos
            Debug.Log("Hay que reiniciar los visible");
            ResetAllVisibles();
        }
        
        allVisible.Clear();
        actual = grid.NodeFromWorldPoint(transform.position);

        VisitingNeighbours(actual);

        for (int i = 0; i < EnemyStats.cantidadMovimientos; i++)
        {

            Nodo n = cola.Dequeue();

            VisitingNeighbours(n);
        }

        cola.Clear();
        
        
    }

    void ResetAllVisibles()
    {
        foreach(Nodo n in allVisible)
        {
            n.prefab.GetComponent<Renderer>().material.color = Color.grey;
        }
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
                cola.Enqueue(aux[i]);
            }
        }
        return aux;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyVisibility();
        
    }
}
