using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unidad : MonoBehaviour
{
    public Enumerados.Position posicion { get; }

    public Unidad(Enumerados.Position posicion)
    {
        this.posicion = posicion;
    }
}
