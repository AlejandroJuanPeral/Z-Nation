using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyValues : MonoBehaviour
{
    //PARA LA ENTIDAD ENEMIGO

    public static int alimentos = 60;
    public static int materiales = 30;
    public static int cantBarracones = 0;
    public static int numTotalUnidades = 0;

    public static int numUnidadesCiudad = 00;

    public static GameObject explorer;

    public static List<GameObject> totalGroups = new List<GameObject>();

    public PlayerManager player;
    public Decisiones decision;
    public Text Resources;


    
    public IEnumerator TurnoEnemigo()
    {
        decision.DecisionGrupo();
        yield return new WaitForSeconds(3f);

        decision.DecisionesCiudad();
        ///CORRRUTINA
        /////llamar start player turn
        yield return new WaitForSeconds(2f);
        Resources.text = "Alimento: " + alimentos.ToString("0000") + " Rec.Construcción: " + materiales.ToString("0000") + '\n' + " Unidades: " + numTotalUnidades + " / " + cantBarracones*3;
        player.PlayerTurn();
    }

    public void LlamarCorutinaEnemigo()
    {
        alimentos += 10;
        alimentos-= numTotalUnidades;
        Resources.text = "Alimento: " + alimentos.ToString("0000") + " Rec.Construcción: " + materiales.ToString("0000") + '\n' + " Unidades: " + numTotalUnidades + " / " + cantBarracones*3;
        //Debug.Log(alimentos);

        foreach(GameObject g in totalGroups)
        {
            g.GetComponent<EnemyMovement>().NewTurn();
        }
        explorer.GetComponent<EnemyExplorerMovement>().NewTurn();
        explorer.GetComponent<EnemyExplorerMovement>().move = true;

        StartCoroutine("TurnoEnemigo");
    }

}
