using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grupo : MonoBehaviour
{
    public Enumerados.Priorities prioridad { get; set; }

    public Grupo(Enumerados.Priorities prioridad)
    {
        this.prioridad = prioridad;
    }

    public List<Unidad> Group = new List<Unidad>();
}
