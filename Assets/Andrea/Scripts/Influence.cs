using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Influence : MonoBehaviour
{
    public ResourceStat.TypeResource typeResource;

    public int maxLevelSearch;

    public bool recolectado = false;

    Grid grid;
    Nodo actual;

    


    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GameManager").GetComponent<Grid>();

        actual = grid.NodeFromWorldPoint(transform.position);

        actual.resourceCost = (int) typeResource;

        actual.objectInNode = this.gameObject;


        actual.prefab.GetComponent<Renderer>().material.color = Color.yellow;

        ExpandInfluence();


    }

    void ExpandInfluence()
    {
        List<Nodo> aux = grid.GetNeighboursInLevel(actual, maxLevelSearch);

        int maxDistance = aux[0].GetDistance(actual);

        int niveles = maxDistance / maxLevelSearch;
       
        for (int i = 0; i < aux.Count; i++)
        {
            int distance = aux[i].GetDistance(actual);

            int cont = 2;

            if(distance > 0 && distance < niveles + niveles / 2)
            {
                cont = 2;
            }

            else if(distance > niveles && distance < niveles * 2 + +niveles / 4)
            {
                cont = 4;
            }

            else if(distance > niveles * 2 && distance < niveles * 3 + niveles / 8)
            {
                cont = 8;
            }
            else
            {
                cont = 16;
            }

            int newValue = (int)typeResource / cont ;
           if (newValue < 1) //se llega a la despreciable
           {
               continue;
           }
            aux[i].resourceCost = newValue;

            aux[i].prefab.GetComponent<Renderer>().material.color = Color.Lerp( Color.yellow, Color.white, 1f /aux[i].resourceCost);
               

            Debug.Log(" nivel " + niveles + " valor " + aux[i].resourceCost);

        }
    }

    public void DestroyThis()
    {
        List<Nodo> aux = grid.GetNeighboursInLevel(actual, maxLevelSearch);
        actual.objectInNode = null;
        actual.resourceCost = -1;
        actual.prefab.GetComponent<Renderer>().material.color = Color.black;
        foreach(Nodo n in aux)
        {
            n.resourceCost = -1;
            n.prefab.GetComponent<Renderer>().material.color = Color.black;
            //n.objectInNode = null;
        }
        Destroy(this.gameObject,0.5f);
        actual.objectInNode = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
