using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject FinishButton, CityPanel, RecoursesPanel, MergePanel, UnitPanel;
    public Text UnitMovement, BarraconText, UnitText, RecourseText;
    public int Food, ConRecourse, Units, MaxUnits, LevelBarracon,aux;
    public List<GameObject> Groups = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Food = 50;
        ConRecourse = 30;
        Units = 0;
        LevelBarracon = 0;
        MaxUnits = LevelBarracon * 3;
        aux = 0;
        RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + ConRecourse.ToString("0000") + "/n Unidades: " + Units + " / " + MaxUnits;
    }

    // Update is called once per frame

    void PlayerTurn()
    {
        Food += 10;
        RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + ConRecourse.ToString("0000") + "/n Unidades: " + Units + " / " + MaxUnits;
        FinishButton.SetActive(true);
        if (Groups.Count > 0)
        {
            aux = 0;
            UnitPanel.SetActive(true);
            UnitMovement.text = "";
            //llama move grupo1
            //Groups[aux].Move;
        }
        else
        {
            CityPanel.SetActive(true);
        }
    }
    public void NextGroup()
    {
        aux = aux + 1;

        if (Groups.Count > aux)
        {
            UnitMovement.text = "";
            //llama move grupo
            //Groups[aux].Move;
        }
        else
        {
            UnitPanel.SetActive(false);

            CityPanel.SetActive(true);
        }
    }
    public void FinishTurn()
    {
        CityPanel.SetActive(false);

        FinishButton.SetActive(false);
    }
    public void UpdateBarracon()
    {
        if (ConRecourse >= (LevelBarracon*30)*0.5 + 30)
        {
            LevelBarracon++;
            ConRecourse -= Mathf.RoundToInt((LevelBarracon * 30) * 0.5f + 30);
            BarraconText.text = "Barracon(nv" + LevelBarracon + ")/n" + Mathf.RoundToInt((LevelBarracon * 30) * 0.5f + 30);
            MaxUnits += 3; 
            UnitText.text = "Unidades (" + Units + "/" + MaxUnits + ")/n 10";
            RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + ConRecourse.ToString("0000") + "/n Unidades: " + Units + " / " + MaxUnits;
        
        }
    }
    public void NewUnit()
    {
        if(Food >= 10 && Units < MaxUnits)
        {
            Units++;
            Food -= 10;
            UnitText.text = "Unidades (" + Units + "/" + MaxUnits + ")/n 10";
            RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + ConRecourse.ToString("0000") + "/n Unidades: " + Units + " / " + MaxUnits;
        }

    }
}
