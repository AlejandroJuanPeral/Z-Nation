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

    public List<Nodo> allVisible;

    int levelIndex = 0;

    Nodo actual;

    Queue<Nodo> cola;

    int index;

    int cantNodos = EnemyStats.cantidadMovimientos;

    public GameObject ciudadPropia;

    public bool withNodeDecision;
    public Nodo decisionNode;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        ciudadPropia = GameObject.FindWithTag("CityEnemy");
        grid = GameObject.Find("GameManager").GetComponent<Grid>();
        allVisible = new List<Nodo>();
        index = 0;

        actual = grid.NodeFromWorldPoint(transform.position);
        vecinos = grid.GetNeighbours(actual);

        List<Nodo> aux = new List<Nodo>();
        List<Nodo> aux2 = new List<Nodo>();

        cola = new Queue<Nodo>();


        anim = GetComponent<Animator>();
        //aux = VisitingNeighbours(actual);

        EnemyVisibility();




    }

    void EnemyVisibility()
    {

        if (allVisible.Count > 0)
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

        Nodo maxValue = grid.NodeFromWorldPoint(ciudadPropia.transform.position);

        foreach (Nodo n in allVisible)
        {
            if (n.resourceCost != -1)
            {
                if (n.resourceCost > maxValue.resourceCost)
                {
                    maxValue = n;
                   
                }
            }
            if(n.objectInNode != null && n.objectInNode.tag == "Player")
            {
                if(n.objectInNode.GetComponent<PlayerStats>().numComponentesGrupo < this.gameObject.GetComponent<EnemyStats>().numComponentesGrupo)
                {
                    maxValue = n;
                    return maxValue;
                }
            }
           // if(n.objectInNode.tag == "CityPlayer")
           // {
           //     //Attack city
           // }

        }
        if(maxValue != grid.NodeFromWorldPoint(ciudadPropia.transform.position))
        {
            decisionNode = maxValue;
            withNodeDecision = true;
            return maxValue;
        }
        else
        {
            if (this.gameObject.GetComponent<EnemyStats>().prioridad == Enumerados.Priorities.Alimento)
            {
                if (maxValue == grid.NodeFromWorldPoint(ciudadPropia.transform.position) && EnemyExplorerMovement.nodosForVisitingFood.Count > 0)
                {
                    maxValue = GetNodoExplorerCercano(EnemyExplorerMovement.nodosForVisitingFood);//Encontramos el nodo más cercano de los que ha visto el explorador y ese será
                }
                return maxValue;
            }
            else if (this.gameObject.GetComponent<EnemyStats>().prioridad == Enumerados.Priorities.Materiales)
            {
                if (maxValue == grid.NodeFromWorldPoint(ciudadPropia.transform.position) && EnemyExplorerMovement.nodosForVisitingResources.Count > 0)
                {
                    maxValue = GetNodoExplorerCercano(EnemyExplorerMovement.nodosForVisitingResources);//Encontramos el nodo más cercano de los que ha visto el explorador y ese será
                }
                return maxValue;
            }
            else
            {
                if (EnemyExplorerMovement.nodosForVisitingFood.Count > 0)
                {
                    maxValue = GetNodoExplorerCercano(EnemyExplorerMovement.nodosForVisitingFood);
                }
                else if (EnemyExplorerMovement.nodosForVisitingResources.Count > 0)
                {
                    maxValue = GetNodoExplorerCercano(EnemyExplorerMovement.nodosForVisitingResources);
                }
                else
                {
                    maxValue = grid.NodeFromWorldPoint(ciudadPropia.transform.position);
                }

                return maxValue;
            }
        }

        

    }
    
    public void AttackPlayer(GameObject player)
    {
        decisionNode = grid.NodeFromWorldPoint(player.transform.position);
        withNodeDecision = true;
        int numGroupPlayer = player.GetComponent<PlayerStats>().numComponentesGrupo;

        int rdn = Random.Range(0, numGroupPlayer);

        this.gameObject.GetComponent<EnemyStats>().numComponentesGrupo -= rdn;
        Destroy(player);

    }
        
    Nodo GetNodoExplorerCercano( List<Nodo> aux)
    {

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



    public void MoveTo(Nodo n)
    {
        if(n != null)
        {
            Nodo anteriorAux = grid.NodeFromWorldPoint(transform.position);


            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, n.worldPosition, EnemyStats.speed * Time.deltaTime);


            Nodo actualAux = grid.NodeFromWorldPoint(transform.position);

            if (anteriorAux == null)
            {
                anteriorAux = actualAux;
            }

            if (actualAux != anteriorAux)
            {
                cantNodos--;
                if (cantNodos == 0)
                {
                    move = false;
                    anim.SetBool("isRunning", false);
                    Debug.Log("Se ha terminado el turno ");
                }
            }

            if (actualAux == n)
            {
                Debug.Log("Llegado al destino");
                decisionNode = null;
                withNodeDecision = false;


                foreach (GameObject g in EnemyValues.totalGroups)
                {
                    if (g != null)
                    {
                        Debug.Log("Distancia: " + Vector3.Distance(g.transform.position, this.transform.position));

                        if (g == this.gameObject)
                        {
                            continue;
                        }
                        else if (Vector3.Distance(g.transform.position, this.transform.position) < 0.3f)
                        {
                            g.GetComponent<EnemyStats>().numComponentesGrupo += this.gameObject.GetComponent<EnemyStats>().numComponentesGrupo;
                            EnemyValues.totalGroups.Remove(this.gameObject);
                            Destroy(this.gameObject);
                        }
                    }

                }

                if (n.objectInNode != null)
                {

                    if (n.objectInNode.tag == "Food")
                    {
                        Debug.Log("Alimento ");
                        if (EnemyExplorerMovement.nodosForVisitingFood.Contains(n))
                        {
                            EnemyExplorerMovement.nodosForVisitingFood.Remove(n);

                        }
                        n.objectInNode.gameObject.GetComponent<Influence>().DestroyThis();

                        EnemyValues.alimentos += 30;
                        this.gameObject.GetComponent<EnemyStats>().prioridad = Enumerados.Priorities.None;
                    }

                    else if (n.objectInNode.tag == "Resource")
                    {
                        Debug.Log("resource ");
                        if (EnemyExplorerMovement.nodosForVisitingResources.Contains(n))
                        {
                            EnemyExplorerMovement.nodosForVisitingResources.Remove(n);

                        }
                        n.objectInNode.gameObject.GetComponent<Influence>().DestroyThis();

                        EnemyValues.materiales += 30;
                        this.gameObject.GetComponent<EnemyStats>().prioridad = Enumerados.Priorities.None;
                    }

                    else if (n.objectInNode.tag == "CityEnemy")
                    {
                        Debug.Log("Pa dentro");
                        EnemyValues.numUnidadesCiudad += this.gameObject.GetComponent<EnemyStats>().numComponentesGrupo;
                        EnemyValues.totalGroups.Remove(this.gameObject);
                        Destroy(this.gameObject);
                    }
                    else if(n.objectInNode.tag == "CityPlayer")
                    {
                        n.objectInNode.GetComponent<PlayerManager>().GameOver();
                    }


                }

                else
                {
                    n.objectInNode = this.gameObject;

                }
                //HAY QUE HACER UNA FUNCION QUE RESETEE EL NODO AL QUE VA DESPUES DE QUE LLEGUE

            }
            EnemyVisibility();



        }

    }
    //Para que sea un nuevo turno se llama a esta función y se pone a true el bool move
    public void NewTurn()
    {
        cantNodos = EnemyStats.cantidadMovimientos;
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

                if(aux[i].objectInNode == null)
                {
                    continue;
                }

                if(aux[i].objectInNode.tag == "Food")
                {
                    if (!EnemyExplorerMovement.nodosForVisitingFood.Contains(aux[i]))
                    {
                        EnemyExplorerMovement.nodosForVisitingFood.Add(aux[i]);
                    }
                }
                if (aux[i].objectInNode.tag == "Resource")
                {
                    if (!EnemyExplorerMovement.nodosForVisitingResources.Contains(aux[i]))
                    {
                        EnemyExplorerMovement.nodosForVisitingResources.Add(aux[i]);
                    }
                }

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
            anim.SetBool("isRunning", true);

            if (withNodeDecision && decisionNode != null)
            {

                MoveTo(decisionNode);
            }
            else
            {
                MoveTo(DecisionMovement());

            }

        }




    }
}
