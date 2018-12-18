using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decisiones : MonoBehaviour
{
    public GameObject ciudadEnemiga;
    public Grid grid;


    public int coste_Barracon;
    public int coste_Unidad = 10;//alimento
    public int coste_MantenimientoUnidad = 1;//alimento

    public GameObject groupPrefab;

    public GameObject explorer;
    int indexExplorer = 0;

    public Nodo nodoCiudadEnemiga;

    private void Start()
    {
        coste_Barracon = CosteBarracon();
        grid = GameObject.Find("GameManager").GetComponent<Grid>();
        EnemyValues.explorer = explorer;

        nodoCiudadEnemiga = grid.NodeFromWorldPoint(this.transform.position);
        nodoCiudadEnemiga.objectInNode = this.gameObject;
    }
    //Ejecutarse mientras tengamos material y alimento
    public void DecisionesCiudad() {
        nodoCiudadEnemiga.objectInNode = this.gameObject;

        if (EnemyValues.numUnidadesCiudad > 10)
        {
        	EnviarAtaque();
		}

        if (AlimentoSuficienteParaMantener(EnemyValues.alimentos, EnemyValues.numTotalUnidades))
        {
            if (BarraconesLibres(EnemyValues.cantBarracones, EnemyValues.numTotalUnidades))
            {
                if (AlimentoSuficienteParaCrear(EnemyValues.alimentos, coste_Unidad))
                {
                    NuevaUnidad();
                    DecisionesCiudad();
                }
                else //No tiene alimento suficiente para mantener las unidades + crear nuevas
                {
                    ProcesarGrupoParado(Enumerados.Priorities.Alimento);
                }
            }
            else //No hay barracones sin ocupar
            {
                if (MaterialSuficiente(EnemyValues.materiales))
                {
                    NuevoBarracon();
                    DecisionesCiudad();
                }
                else //No tiene materiales suficiente para construir
                {
                    ProcesarGrupoParado(Enumerados.Priorities.Materiales);
                }
            }
        }
        else  //No tiene alimento suficiente para mantener las unidades
        {
            ProcesarGrupoParado(Enumerados.Priorities.Alimento);
        }

    }



    public void DecisionGrupo() {

        nodoCiudadEnemiga.objectInNode = this.gameObject;
        if (UnidadesFueraCiudad())
        {
            foreach (GameObject grupo in EnemyValues.totalGroups)
            {
                GameObject grupoEnemigo = EnemigoCerca(grupo);
                if (grupoEnemigo!=null)
                {
                    if (!EnemigoFuerte(grupo, grupoEnemigo))
                    {
                        grupo.GetComponent<EnemyMovement>().AttackPlayer(grupoEnemigo);
                        grupo.GetComponent<EnemyMovement>().move = true;


                    }
                }
                else
                {
                    grupo.GetComponent<EnemyMovement>().move = true;
                }
               
                //mover el grupo--------------------------------------------------------------------------------------------------------
            }
        }

    }

 
    /*public Decisiones(int alimento, int materiales, int barracones, int unidades, List<GameObject> grupos)
    {
        this.alimento = alimento;
        this.materiales = materiales;
        this.barracones = barracones;
        unidadesTotales = unidades;
        this.grupos = grupos;



        if (AlimentoSuficienteParaMantener(alimento, unidades))
        {
            if (BarraconesLibres(barracones, unidades))
            {
                if (AlimentoSuficienteParaCrear(alimento, unidades))
                {
                    NuevaUnidad();
                }
                else //No tiene alimento suficiente para mantener las unidades + crear nuevas
                {
                    ProcesarGrupoParado(grupos, Enumerados.Priorities.Alimento);
                }
            }
            else //No hay barracones sin ocupar
            {
                if (MaterialSuficiente(materiales))
                {
                    NuevoBarracon();
                }
                else //No tiene materiales suficiente para construir
                {
                    ProcesarGrupoParado(grupos, Enumerados.Priorities.Materiales);
                }
            }
        }
        else //No tiene alimento suficiente para mantener las unidades
        {
            ProcesarGrupoParado(grupos, Enumerados.Priorities.Alimento);
        }


        if (unidadesDentroCiudad > 10)
        {
            int random = Random.Range(0, 3);
            if (random == 2)
            {
                int resto = unidades % 10;
                int numGrupos = unidades / 10;
                if (resto > 1)//Porque quiero que se queden al menos 2 unidades en la ciudad
                {
                    for (int i = 0; i < numGrupos; i++)
                    {
                        GrupoAtaque(); //Enviar (unidades.Count/10) grupos de 10 unidades a atacar la ciudad enemiga
                    }
                   
                }
            }
        }


        if (UnidadesFueraCiudad(grupos))
        {
            foreach (GameObject grupo in grupos)
            {
                GameObject grupoEnemigo = EnemigoCerca(grupo);
                if (grupoEnemigo)
                {
                    if (!EnemigoFuerte(grupo, grupoEnemigo))
                    {
                        grupo.GetComponent<EnemyMovement>().AttackPlayer(grupoEnemigo);
                    }
                }
            }          
        }


    }*/
   

    public bool AlimentoSuficienteParaMantener(int alimento, int unidades)
    {
        return alimento > unidades * 2;
    }

    public bool MaterialSuficiente(int materiales)
    {
        return materiales >= coste_Barracon;
    }

    public bool BarraconesLibres(int barracones, int unidades)
    {
        return barracones > unidades/3;
    }

    public bool AlimentoSuficienteParaCrear(int alimento, int unidades)
    {
        return alimento > (unidades * 2) + coste_Unidad;
    }

    public GameObject GruposFueraSinPrioridad()
    {
        foreach (GameObject grupo in EnemyValues.totalGroups)
        {
            if (grupo.GetComponent<EnemyStats>().prioridad == Enumerados.Priorities.None) return grupo;
        }

        return null;
    }

    public void ProcesarGrupoParado(Enumerados.Priorities prioridad)
    {
        GameObject grupoParado = GruposFueraSinPrioridad();

        if (grupoParado == null)
        {
        	if (EnemyValues.numUnidadesCiudad > 1 && EnemyValues.numUnidadesCiudad > EnemyValues.numTotalUnidades/2.25)
            {
                int rdn;
                if (EnemyValues.numUnidadesCiudad > 9)
                {
                    rdn = Random.Range(1, 10);
                }
                else
                {
                    rdn = Random.Range(1, EnemyValues.numUnidadesCiudad - 1);
                }

                NuevoGrupo(rdn, prioridad);    //Llamar a la función CREAR GRUPO DE UNIDADES (numComponentesGrupo = nº unidades en el grupo)
                DecisionesCiudad();				//Se le asignará una prioridad y llamará a la función BUSCAR X/Y.

            }                                   
        	else
        	PasarTurno();
        }
        else if (grupoParado != null)
        {
            grupoParado.GetComponent<EnemyStats>().prioridad = prioridad; //Llamar a la función BUSCAR X/Y
            grupoParado.GetComponent<EnemyMovement>().move = true;
            DecisionesCiudad();

        }
    }

    public bool UnidadesFueraCiudad()
    {
        return EnemyValues.totalGroups.Count > 0;
    }

    public bool GrupoConPrioridad(GameObject grupo)
    {
        return grupo.GetComponent<EnemyStats>().prioridad != Enumerados.Priorities.None;
    }

    public GameObject EnemigoCerca(GameObject grupo)
    {
        List<Nodo> aux = grupo.GetComponent<EnemyMovement>().allVisible;
        foreach (Nodo n in aux)
        {
            if(n.objectInNode == null)
            {
                continue;
            }
            else if (n.objectInNode.tag == "Player")
            {
                return n.objectInNode;
            }
        }
        return null; //Hay enemigos visibles? Estan a distancia atacable?
    }

    public bool EnemigoFuerte(GameObject grupo, GameObject grupoEnemigo)
    {
        return grupo.GetComponent<EnemyStats>().numComponentesGrupo < grupoEnemigo.GetComponent<PlayerStats>().numComponentesGrupo;
    }

    public int CosteBarracon()
    {
        return (int)((EnemyValues.cantBarracones * 30) * 0.5f + 30);
    }

    public void NuevoBarracon()
    {
        EnemyValues.materiales -= coste_Barracon;
        EnemyValues.cantBarracones++;
    }

    public void NuevaUnidad()
    {
       	EnemyValues.numTotalUnidades++;
        EnemyValues.numUnidadesCiudad++;
        EnemyValues.alimentos -= 10;
    }

    public void NuevoGrupo(int num, Enumerados.Priorities prioridad)
    {        	
        GameObject Group = Instantiate(groupPrefab, this.transform.position, Quaternion.identity);
        //Group.AddComponent<EnemyStats>();
        Group.GetComponent<EnemyStats>().prioridad = prioridad;
       	Group.GetComponent<EnemyStats>().numComponentesGrupo = num;
        EnemyValues.numUnidadesCiudad -= num;
        //Group.GetComponent<EnemyMovement>().move = true;
        EnemyValues.totalGroups.Add(Group);
    }

    public void GrupoAtaque()
    {
        GameObject Group = Instantiate(groupPrefab, this.transform.position, Quaternion.identity);

        Group.GetComponent<EnemyStats>().numComponentesGrupo = 10;
        Group.GetComponent<EnemyMovement>().decisionNode = grid.NodeFromWorldPoint(ciudadEnemiga.transform.position);
        Group.GetComponent<EnemyMovement>().withNodeDecision = true;
        EnemyValues.numUnidadesCiudad -= 10;
        EnemyValues.totalGroups.Add(Group);
       // Group.GetComponent<EnemyMovement>().MoveTo(grid.NodeFromWorldPoint(ciudadEnemiga.transform.position));
        
    }

    public void PasarTurno()
    {
        foreach (GameObject g in EnemyValues.totalGroups)
        {
            g.GetComponent<EnemyMovement>().NewTurn();
        }
    }

    public void EnviarAtaque()
	{
        int random = Random.Range(0, 3);
        if (random == 2)
        {
            int resto = EnemyValues.numUnidadesCiudad % 10;
            int numGrupos = EnemyValues.numUnidadesCiudad / 10;
            if (resto > 1)//Porque quiero que se queden al menos 2 unidades en la ciudad
            {
                for (int i = 0; i < numGrupos; i++)
                {
                    GrupoAtaque(); //Enviar (unidades.Count/10) grupos de 10 unidades a atacar la ciudad enemiga
                }
            }
        }
	}

}

