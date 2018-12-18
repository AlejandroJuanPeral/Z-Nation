using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplorerMovement : MonoBehaviour
{
    //Lanzar una búsqueda en anchura en varios niveles hasta la visualización indicada en EnemyStats
    //En esa búsqueda, buscar el nodo con mayor peso
    public Color colorReachable;
    public Color pathColor;

    

    public bool move;

    public List<Transform> positions = new List<Transform>();

 
    public static List<Nodo> nodosForVisitingFood = new List<Nodo>();
    public static List<Nodo> nodosForVisitingResources = new List<Nodo>();

    Grid grid;

    List<Nodo> vecinos;

    List<Nodo> allVisible;

    int levelIndex = 0;

    Nodo actual;

    Queue<Nodo> cola;

    int index = 0;

    //Nodos a visitar

    
    bool arrive = false;
    bool finished = false;




    bool reachDestination;

    Transform actualPosition;

    int cantNodos = EnemyStats.movimientoExplorer;

    Animator anim;

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

        anim = GetComponent<Animator>();

        NextPosition();


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

    void NextPosition()
    {
        finished = false;
        if (index > positions.Count)
        {
            Destroy(this.gameObject);
        }
        actualPosition = positions[index];
    }

    void ResetAllVisibles()
    {

        foreach (Nodo n in allVisible)
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
        List<Nodo> aux = grid.GetNeighboursInLevel(n, EnemyStats.movimientoExplorer);


        for (int i = 0; i < aux.Count; i++)
        {
            if (aux[i].prefab.GetComponent<Renderer>().material.color == colorReachable)
            {
                continue;
            }
            else
            {
                aux[i].prefab.GetComponent<Renderer>().material.color = colorReachable;
                allVisible.Add(aux[i]);
                aux[i].isVisibleEnemy = true;
                cola.Enqueue(aux[i]);
                if(aux[i].objectInNode != null)
                {
                    if (aux[i].objectInNode.tag == "Food")
                    {
                        if (!nodosForVisitingFood.Contains(aux[i]))
                        {
                            nodosForVisitingFood.Add(aux[i]);
                        }

                    } else if (aux[i].objectInNode.tag == "Resource")
                    {
                        if (!nodosForVisitingResources.Contains(aux[i]))
                        {
                            nodosForVisitingResources.Add(aux[i]);
                        }
                    }
                    
                }

            }
        }

        return aux;
    }
    //Para que sea un nuevo turno se llama a esta función y se pone a true el bool move
    public void NewTurn()
    {
        cantNodos = EnemyStats.movimientoExplorer;
    }

    void MovementExplorer()
    {
        Nodo anteriorAux = grid.NodeFromWorldPoint(transform.position);


        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, actualPosition.position, EnemyStats.speed * Time.deltaTime);


        Nodo actualAux = grid.NodeFromWorldPoint(transform.position);

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

        if (this.gameObject.transform.position == actualPosition.position)
       {
            
            finished = true;
            index += 1;
            NextPosition();
            anim.SetBool("isRunning", false);

            //Debug.Log("index : " + index);


        }


    }

    // Update is called once per frame
    void Update()
    {

        //VisitingNeighbours(actual);
        EnemyVisibility();

        if(move == true) {
            if (finished == false) //Comprobar si tiene movimientos restantes
            {
                MovementExplorer();
                anim.SetBool("isRunning", true);
            }
        }


    }
}
