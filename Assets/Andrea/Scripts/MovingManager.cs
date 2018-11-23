using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingManager : MonoBehaviour
{
    public static MovingManager instance;

    public Text movementText;

    private Moving selectedNode;

    //Crear variables que influyen en el movimiento

    public bool CanMove
    {
        get
        {
            return PlayerStats.cantidadNodosPorTurno > 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        movementText.text = "Movement left: " + PlayerStats.cantidadNodosPorTurno;
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

    public void MoveOn( Moving node)
    {
        if(PlayerStats.cantidadNodosPorTurno <= 0)
        {
            Debug.Log("Ya no quedan movimientos ");
            return;
        }
        PlayerStats.cantidadNodosPorTurno--;
        movementText.text = "Movement left: " + PlayerStats.cantidadNodosPorTurno;
        //Nos movemos al nodo deseado

        node.gameObject.GetComponent<Renderer>().material.color = Color.cyan;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
