using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Color colorReachable;
    public Color pathColor;
    public bool final;


    Grid grid;

    List<Nodo> allVisible;
    Nodo actual;


    public bool isMoving = false;

    public int cantNodos = PlayerStats.cantidadNodosPorTurno;

    public GameObject manager;
    

    // Start is called before the first frame update
    void Start()
    {
        final = false;
        grid = GameObject.Find("GameManager").GetComponent<Grid>();

        manager = GameObject.FindGameObjectWithTag("CityPlayer");

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
            final = false;
            cantNodos--;
            if (cantNodos == 0)
            {
                isMoving = false;
                Debug.Log("Se ha terminado el turno ");
            }
        }

        if (this.gameObject.transform.position == n.worldPosition && final == false)
        {
            if(n.objectInNode.tag == "Food")
            {
                manager.GetComponent<PlayerManager>().TakeFood(30);
                n.objectInNode.GetComponent<Influence>().recolectado = true;
                n.objectInNode.GetComponent<Influence>().DestroyThis();

            }
            else if(n.objectInNode.tag == "Resource")
            {

                manager.GetComponent<PlayerManager>().TakeResources(30);
                n.objectInNode.GetComponent<Influence>().recolectado = true;
                n.objectInNode.GetComponent<Influence>().DestroyThis();


            }
            else if(n.objectInNode.tag == "Enemy")
            {
                AttackEnemy(n.objectInNode);
            }
            else if(n.objectInNode.tag == "Player")
            {
                manager.GetComponent<PlayerManager>().MergeGroups(this.gameObject, n.objectInNode);
            }
            else if(n.objectInNode.tag == "CityPlayer")
            {
                manager.GetComponent<PlayerManager>().UnitsInCity += this.gameObject.GetComponent<PlayerStats>().numComponentesGrupo;
                Destroy(this.gameObject);
            }
            else if(n.objectInNode.tag == "CityEnemy")
            {
                //ATACAR CIUDAD
            }
            final = true;
           //if (n.objectInNode)
           //{
           //    n.objectInNode.GetComponent<Influence>().DestroyThis();
           //}
        }
        
        PlayerVisibility();


    }

    public void AttackEnemy(GameObject enemy)
    {
        int numGroupEnemy = enemy.GetComponent<EnemyStats>().numComponentesGrupo;
        int numGroupPlayer = this.gameObject.GetComponent<PlayerStats>().numComponentesGrupo;
        int rdn;

        if(numGroupEnemy < numGroupPlayer)
        {
            rdn = Random.Range(0, numGroupEnemy);
            this.gameObject.GetComponent<PlayerStats>().numComponentesGrupo -= rdn;
            Destroy(enemy);
        }
        else if(numGroupEnemy == numGroupPlayer)
        {
            if( Random.Range(0, 1) == 1) //Ganas tu
            {
                rdn = Random.Range(0, numGroupEnemy);
                this.gameObject.GetComponent<PlayerStats>().numComponentesGrupo -= rdn;
                Destroy(enemy);
                if(this.gameObject.GetComponent<PlayerStats>().numComponentesGrupo == 0)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                rdn = Random.Range(0, numGroupPlayer);
                enemy.gameObject.GetComponent<EnemyStats>().numComponentesGrupo -= rdn;
               
                if (enemy.gameObject.GetComponent<EnemyStats>().numComponentesGrupo == 0)
                {
                    Destroy(enemy);
                }

                Destroy(this.gameObject);
            }
        }
        else
        {
            rdn = Random.Range(0, numGroupPlayer);
            enemy.gameObject.GetComponent<EnemyStats>().numComponentesGrupo -= rdn;
            Destroy(this.gameObject);

        }


    }

    // Update is called once per frame
    void Update()
    {
        PlayerVisibility();
    }
}
