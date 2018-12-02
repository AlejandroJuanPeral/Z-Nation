using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Moving : MonoBehaviour
{

  
    public Color reachable;
    public Color unreachable;

    public Vector3 positionOffset;

    public MovingManager moving;


    private Renderer rend;
    private Color startColor;

    private MovingManager movingManager;


    Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        movingManager = MovingManager.instance;

        grid = GameObject.Find("GameManager").GetComponent<Grid>();
    }

    public Vector3 GetMovePosition()
    {
        return positionOffset + transform.position;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!movingManager.CanMove)
        {
            return;
        }



        //Comprobaciones para moverse
        Nodo n = grid.NodeFromWorldPoint(this.transform.position);

        if (n.isVisiblePlayer)
        {
            n.prefab.GetComponent<Renderer>().material.color = Color.cyan;
            //rend.material.color = Color.cyan;
            movingManager.MoveOn(this);
            return;
        }
        
    }

    private void OnMouseExit()
    {
        Nodo n = grid.NodeFromWorldPoint(this.transform.position);

        if (n.isVisiblePlayer)
        {
            rend.material.color = Color.gray;
            return;
        }
        //rend.material.color = Color.blue;
        rend.material.color = startColor;
    }

    private void OnMouseEnter()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Nodo n = grid.NodeFromWorldPoint(this.transform.position);

        if (n.isVisiblePlayer)
        {
            rend.material.color = unreachable;
            return;
        }


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
