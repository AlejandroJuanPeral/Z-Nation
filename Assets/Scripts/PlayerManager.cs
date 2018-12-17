using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject FinishButton, CityPanel, RecoursesPanel, MergePanel, UnitPanel, SeparatePanel, Button, GameManager;
    public Text UnitMovement, BarraconText, UnitText, RecourseText,UnitCity;
    public Slider SepSlider,MergeSlider;
    public int Food, Resources, Units, MaxUnits, UnitsInCity, LevelBarracon,aux;
    public List<GameObject> Groups = new List<GameObject>();
    public GameObject GroupPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
        Food = 50;
        Resources = 30;
        Units = 0;
        LevelBarracon = 0;
        MaxUnits = LevelBarracon * 3;
        aux = 0;
        RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + Resources.ToString("0000") + '\n' + " Unidades: " + Units + " / " + MaxUnits;

        PlayerTurn();
    }
    private void Update()
    {
        if (Groups.Count > 0 && Groups[aux] != null)
        {
            UnitMovement.text = "Movimiento " + Groups[aux].GetComponent<PlayerMovement>().cantNodos;

        }

    }
    // Update is called once per frame
    public void TakeFood(int num)
    {
        Food += num;
        RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + Resources.ToString("0000") + '\n' + " Unidades: " + Units + " / " + MaxUnits;
    }
    public void TakeResources(int num)
    {
        Resources += num;
        RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + Resources.ToString("0000") + '\n' + " Unidades: " + Units + " / " + MaxUnits;
    }
    void PlayerTurn()
    {
        Food += 10;
        Food -= Units;
        RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + Resources.ToString("0000") + '\n' + " Unidades: " + Units + " / " + MaxUnits;
        FinishButton.SetActive(true);
        if (Groups.Count > 0)
        {
            aux = 0;
            UnitPanel.SetActive(true);
            //llama a mover grupo
            
            if (Groups[aux] == null) NextGroup();
            GameManager.GetComponent<MovingManager>().NewAssigmentGroup(Groups[aux]);
            Groups[aux].GetComponent<PlayerMovement>().NextTurn();
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
            if (Groups[aux] == null) NextGroup();
            UnitMovement.text = "";
            GameManager.GetComponent<MovingManager>().NewAssigmentGroup(Groups[aux]);
            Groups[aux].GetComponent<PlayerMovement>().NextTurn();
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
        PlayerTurn();
    }
    public void UpdateBarracon()
    {
        if (Resources >= (LevelBarracon*30)*0.5 + 30)
        {
            Resources -= (int) ((LevelBarracon * 30) * 0.5f + 30);
            LevelBarracon++;
            BarraconText.text = "Barracon(nv" + LevelBarracon + ")\n" + Mathf.RoundToInt((LevelBarracon * 30) * 0.5f + 30);
            MaxUnits += 3; 
            UnitText.text = "Unidades (" + Units + "/" + MaxUnits + ")\n 10";
            RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + Resources.ToString("0000") + '\n' + " Unidades: " + Units + " / " + MaxUnits;
        }
    }
    public void NewUnit()
    {
        if(Food >= 10 && Units < MaxUnits)
        {
            Units++;
            UnitsInCity++;
            Food -= 10;
            UnitCity.text = "Unidades ciudad: " + UnitsInCity;
            UnitText.text = "Unidades (" + Units + "/" + MaxUnits + ")\n 10";
            RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + Resources.ToString("0000") + '\n' + " Unidades: " + Units + " / " + MaxUnits;
        }

    }
    public void NewGroup(int num)
    {
        if (num <= UnitsInCity)
        {
            GameObject Group = Instantiate(GroupPrefab, this.transform.position, Quaternion.identity);
            Group.GetComponent<PlayerStats>().numComponentesGrupo = num;
            Groups.Add(Group);
            UnitsInCity -= num;
            UnitCity.text = "Unidades ciudad: " + UnitsInCity;
        }
    }
    public void Separate()
    {
        SeparatePanel.SetActive(true);
        SepSlider.maxValue = Groups[aux].GetComponent<PlayerStats>().numComponentesGrupo - 1;
        SepSlider.value = Groups[aux].GetComponent<PlayerStats>().numComponentesGrupo - 1;
    }
    public void SeparateNewGroup(int num)
    {
        if (num < Groups[aux].GetComponent<PlayerStats>().numComponentesGrupo)
        {
            GameObject Group = Instantiate(GroupPrefab, this.transform.position, Quaternion.identity);
            Group.GetComponent<PlayerStats>().numComponentesGrupo = num;
            Groups.Add(Group);
            Groups[aux].GetComponent<PlayerStats>().numComponentesGrupo -= num;
        }
    }
    public void MergeGroups(GameObject Group1, GameObject Group2)
    {
        Button.GetComponent<BottonScript>().Group1 = Group1;
        Button.GetComponent<BottonScript>().Group2 = Group2;
        MergePanel.SetActive(true);
        MergeSlider.value = Group1.GetComponent<PlayerStats>().numComponentesGrupo;
        MergeSlider.maxValue = Group2.GetComponent<PlayerStats>().numComponentesGrupo+ Group1.GetComponent<PlayerStats>().numComponentesGrupo;
    }
    public void EnterCity(GameObject Group)
    {
        UnitsInCity += Group.GetComponent<PlayerStats>().numComponentesGrupo;
        Destroy(Group);
    }
}
