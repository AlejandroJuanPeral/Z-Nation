using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decisiones : MonoBehaviour
{
    public int alimento = EnemyValues.alimentos; // Alimento suficiente = Nº Unidades x2
    public int materiales = EnemyValues.materiales;
    public GameObject ciudadEnemiga;
    public Grid grid;

    public int barracones = EnemyValues.cantBarracones;
    public int unidadesTotales = EnemyValues.numTotalUnidades;
    public int unidadesDentroCiudad = EnemyValues.numUnidadesCiudad;
    public List<GameObject> grupos = EnemyValues.totalGroups;

    public int coste_Barracon;
    public int coste_Unidad = 10;//alimento
    public int coste_MantenimientoUnidad = 1;//alimento

    public GameObject groupPrefab;

    private void Start()
    {
        coste_Barracon = CosteBarracon();
        grid = GameObject.Find("GameManager").GetComponent<Grid>();
    }

    public Decisiones(int alimento, int materiales, int barracones, int unidades, List<GameObject> grupos, int coste_Barracon, int coste_Unidad, int coste_MantenimientoUnidad)
    {
        this.alimento = alimento;
        this.materiales = materiales;
        this.barracones = barracones;
        unidadesTotales = unidades;
        this.grupos = grupos;
        this.coste_Barracon = coste_Barracon;
        this.coste_Unidad = coste_Unidad;
        this.coste_MantenimientoUnidad = coste_MantenimientoUnidad;



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
                        //Hay que atacar, vale?----------------------------------------------------------------------------------------------------------------------------------------------------------
                    }
                }
            }          
        }


    }
   

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
        return barracones > unidades/2;
    }

    public bool AlimentoSuficienteParaCrear(int alimento, int unidades)
    {
        return alimento > (unidades * 2) + coste_Unidad;
    }

    public GameObject GruposFueraSinPrioridad(List<GameObject> grupos)
    {
        foreach (GameObject grupo in grupos)
        {
            if (grupo.GetComponent<EnemyStats>().Prioridad == Enumerados.Priorities.None) return grupo;
        }

        return null;
    }

    public void ProcesarGrupoParado(List<GameObject> grupo, Enumerados.Priorities prioridad)
    {
        GameObject grupoParado = GruposFueraSinPrioridad(grupo);
        if (grupoParado)
        {
            grupoParado.GetComponent<EnemyStats>().Prioridad = prioridad; //Llamar a la función BUSCAR X/Y
        }
        else
        {
            if (unidadesDentroCiudad > 1)
            {
                int rdn;
                if (unidadesDentroCiudad > 9)
                {
                    rdn = Random.Range(1, 10);
                }
                else
                {
                    rdn = Random.Range(1, unidadesDentroCiudad - 1);
                }

                NuevoGrupo(rdn, prioridad);    //Llamar a la función CREAR GRUPO DE UNIDADES (numComponentesGrupo = nº unidades en el grupo)
                                            //Se le asignará una prioridad y llamará a la función BUSCAR X/Y.
            }
        }
    }

    public bool UnidadesFueraCiudad(List<GameObject> grupos)
    {
        return grupos.Count > 0;
    }

    public bool GrupoConPrioridad(GameObject grupo)
    {
        return grupo.GetComponent<EnemyStats>().Prioridad != Enumerados.Priorities.None;
    }

    public GameObject EnemigoCerca(GameObject grupo)
    {
        List<Nodo> aux = grupo.GetComponent<EnemyMovement>().allVisible;
        foreach(Nodo n in aux)
        {
            if(n.objectInNode.tag == "Player")
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
        return (int) ((barracones * 30) * 0.5f + 30);
    }

    public void NuevoBarracon()
    {
        materiales -= coste_Barracon;
        barracones++;
    }

    public void NuevaUnidad()
    {
        unidadesDentroCiudad++;
        alimento -= 10;
    }

    public void NuevoGrupo(int num, Enumerados.Priorities prioridad)
    {
        GameObject Group = Instantiate(groupPrefab,this.transform.position, Quaternion.identity);
        Group.GetComponent<EnemyStats>().Prioridad = prioridad;
        Group.GetComponent<EnemyStats>().numComponentesGrupo = num;
        unidadesDentroCiudad -= num;
        grupos.Add(Group);
    }

    public void GrupoAtaque()
    {
        GameObject Group = Instantiate(groupPrefab, this.transform.position, Quaternion.identity);
        Group.GetComponent<EnemyMovement>().MoveTo(grid.NodeFromWorldPoint(ciudadEnemiga.transform.position));
        Group.GetComponent<EnemyStats>().numComponentesGrupo = 10;
        unidadesDentroCiudad -= 10;
        grupos.Add(Group);
    }

}


