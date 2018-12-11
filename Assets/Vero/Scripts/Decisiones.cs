using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decisiones : MonoBehaviour
{
    public int alimento; // Alimento suficiente = Nº Unidades x2
    public int materiales;

    public int barracones;
    public List<Unidad> unidades = new List<Unidad>();
    public List<Grupo> grupo = new List<Grupo>();

    public int coste_Barracon;
    public int coste_Unidad;
    public int coste_MantenimientoUnidad;

    public Decisiones(int alimento, int materiales, int barracones, List<Unidad> unidades, List<Grupo> grupos, int coste_Barracon, int coste_Unidad, int coste_MantenimientoUnidad)
    {
        this.alimento = alimento;
        this.materiales = materiales;
        this.barracones = barracones;
        this.unidades = unidades;
        this.grupo = grupos;
        this.coste_Barracon = coste_Barracon;
        this.coste_Unidad = coste_Unidad;
        this.coste_MantenimientoUnidad = coste_MantenimientoUnidad;



        if (AlimentoSuficienteParaMantener(alimento, unidades))
        {
            if (BarraconesLibres(barracones, unidades))
            {
                if (AlimentoSuficienteParaCrear(alimento, unidades))
                {
                    //Llamada a la función CREAR UNIDAD.
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
                    //Llamada a la función CREAR BARRACÓN
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


        if (CiudadPoblada(unidades))
        {
            int random = Random.Range(0, 3);
            if (random == 2)
            {
                int resto = unidades.Count % 10;
                if (resto > 1)//Porque quiero que se queden al menos 2 unidades en la ciudad
                {
                    //Enviar (unidades.Count/10) grupos de 10 unidades a atacar la ciudad enemiga
                }
            }
        }


        if (UnidadesFueraCiudad(grupos))
        {
            foreach (Grupo grupo in grupos)
            {
                Grupo grupoEnemigo = EnemigoCerca(grupo);
                if (grupoEnemigo)
                {
                    if (EnemigoFuerte(grupo, grupoEnemigo))
                    {
                        if (GrupoConPrioridad(grupo))
                        {
                            //Llamar a la función HUYE COBARDE VILLANO O TE DEJARÉ LA MARCA DE LOS DEDOS DE MI MANO
                        }
                        else
                        {
                            //Llamar a la función VUELVE A LA CIUDAD
                        }
                    }
                    else
                    {
                        //Llamar a la función AL ATAQUEEE
                    }
                }
            }          
        }


    }
   


    public bool AlimentoSuficienteParaMantener(int alimento, List<Unidad> unidades)
    {
        return alimento > unidades.Count * 2;
    }

    public bool MaterialSuficiente(int materiales)
    {
        return materiales >= coste_Barracon;
    }

    public bool BarraconesLibres(int barracones, List<Unidad> unidades)
    {
        return barracones > unidades.Count;
    }

    public bool AlimentoSuficienteParaCrear(int alimento, List<Unidad> unidades)
    {
        return alimento > (unidades.Count * 2) + coste_Unidad;
    }

    public Grupo GruposFueraSinPrioridad(List<Grupo> grupos)
    {
        foreach (Grupo grupo in grupos)
        {
            if (grupo.prioridad == Enumerados.Priorities.None) return grupo;
        }

        return null;
    }

    public void ProcesarGrupoParado(List<Grupo> grupo, Enumerados.Priorities prioridad)
    {
        Grupo grupoParado = GruposFueraSinPrioridad(grupo);
        if (grupoParado)
        {
            grupoParado.prioridad = prioridad;
            //Llamar a la función BUSCAR X/Y
        }
        else
        {
            if (UnidadesDentroCiudad(unidades))
            {
                //Llamar a la función CREAR GRUPO DE UNIDADES
                //Creará grupos de tamaño random entre 1 y (máx unidades en la ciudad && máx unidades por grupo)
                //Se le asignará una prioridad y llamará a la función BUSCAR X/Y.
            }
        }
    }

    public bool UnidadesDentroCiudad(List<Unidad> unidades)
    {
        foreach (Unidad unidad in unidades)
        {
            if (unidad.posicion == Enumerados.Position.Ciudad) return true;
        }

        return false;
    }
    
    public bool CiudadPoblada(List<Unidad> unidades)
    {
        int poblada = 0;
        foreach (Unidad unidad in unidades)
        {
            if (unidad.posicion == Enumerados.Position.Ciudad)
            {
                poblada++;
            }
        }

        return poblada >= 10;
    }

    public bool UnidadesFueraCiudad(List<Grupo> grupos)
    {
        return grupos.Count > 0;
    }

    public bool GrupoConPrioridad(Grupo grupo)
    {
        return grupo.prioridad != Enumerados.Priorities.None;
    }

    public Grupo EnemigoCerca(Grupo grupo)
    {
        return null; //Hay enemigos visibles? Estan a distancia atacable?
    }

    public bool EnemigoFuerte(Grupo grupo, Grupo grupoEnemigo)
    {
        return grupo.Group.Count < grupoEnemigo.Group.Count;
    }

}


