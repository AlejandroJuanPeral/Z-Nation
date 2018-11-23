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

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        movingManager = MovingManager.instance;
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

        movingManager.MoveOn(this);
    }

    private void OnMouseExit()
    {
        //rend.material.color = Color.blue;
        rend.material.color = startColor;
    }

    private void OnMouseEnter()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

      // if (!moving.CanMove)
      // {
      //     return;
      // }

        rend.material.color = unreachable;


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
