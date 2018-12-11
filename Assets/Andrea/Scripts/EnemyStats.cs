using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    //PARA CADA GRUPO


    public static int cantidadMovimientos = 5;

    public static int movimientoExplorer = 8;

    public int numComponentesGrupo = 1;

    public static float speed = 10f; 

    public Enumerados.Priorities Prioridad { get; set; }

    public EnemyStats(Enumerados.Priorities priori)
    {
        Prioridad = priori;
    }

    

}
