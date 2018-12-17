using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingManager : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMovement;


    public static MovingManager instance;


    

    private Nodo selectedNode;

    Grid grid;

    


    //Crear variables que influyen en el movimiento

    public bool CanMove
    {
        get
        {
            return playerMovement.cantNodos > 0;
        }
    }

    public void NewAssigmentGroup(GameObject group)
    {
        player = group;
        playerMovement = player.GetComponent<PlayerMovement>();
    }
    // Start is called before the first frame update
    void Start()
    {

        //playerMovement = player.GetComponent<PlayerMovement>();

        grid = GameObject.Find("GameManager").GetComponent<Grid>();
    }
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than a one MovingManager in scene!");
            return;
        }
        instance = this;
    }

    public void MoveOn(Moving node)
    {
        if (playerMovement.cantNodos <= 0)
        {
            Debug.Log("Ya no quedan movimientos ");
            return;
        }
        //PlayerStats.cantidadNodosPorTurno--;
        //Nos movemos al nodo deseado

        node.gameObject.GetComponent<Renderer>().material.color = Color.cyan;


        selectedNode = grid.NodeFromWorldPoint(node.gameObject.transform.position);

        playerMovement.isMoving = true;

        





    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if (playerMovement.isMoving)
            {
                playerMovement.MoveTo(selectedNode);
            }
        }
    
    }
}
