using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyValues : MonoBehaviour
{
    //PARA LA ENTIDAD ENEMIGO

    public static int alimentos = 100;
    public static int materiales = 10;
    public static int cantBarracones = 1;
    public static int numTotalUnidades = 0;

    public static int numUnidadesCiudad = 00;

    public static List<GameObject> totalGroups = new List<GameObject>();

    public PlayerManager player;
    public Decisiones decision;

    public IEnumerator TurnoEnemigo()
    {
        
        decision.DecisionGrupo();
        ///CORRRUTINA
        /////llamar start player turn
        yield return new WaitForSeconds(5f);
        player.PlayerTurn();
    }

    public void LlamarCorutinaEnemigo()
    {
        StartCoroutine("TurnoEnemigo");
    }

}
