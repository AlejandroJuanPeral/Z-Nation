using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject FinishButton, CityPanel, RecoursesPanel, MergePanel, UnitPanel, SeparatePanel, Button, GameManager,gAMEoVERpANEL;
    public Text UnitMovement, BarraconText, UnitText, RecourseText,UnitCity;
    public Slider SepSlider,MergeSlider;
    public int Food, Resources, Units, MaxUnits, UnitsInCity, LevelBarracon,aux;
    public List<GameObject> Groups = new List<GameObject>();
    public GameObject GroupPrefab;

    public EnemyValues enemigo;

    public Grid grid;

    public Nodo nodoCiudad;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GameManager").GetComponent<Grid>();

        Food = 50;
        Resources = 30;
        Units = 0;
        LevelBarracon = 0;
        MaxUnits = LevelBarracon * 3;
        aux = 0;
        RecourseText.text = "Alimento: " + Food.ToString("0000") + " Rec.Construcción: " + Resources.ToString("0000") + '\n' + " Unidades: " + Units + " / " + MaxUnits;

        nodoCiudad = grid.NodeFromWorldPoint(this.transform.position);
        nodoCiudad.objectInNode = this.gameObject;

        PlayerTurn();
    }
    private void Update()
    {
        nodoCiudad.objectInNode = this.gameObject;
        
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
    public void PlayerTurn()
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
        SeparatePanel.SetActive(false);
        MergePanel.SetActive(false);
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
        SeparatePanel.SetActive(false);
        MergePanel.SetActive(false);
        CityPanel.SetActive(false);

        FinishButton.SetActive(false);
        //PlayerTurn();
        enemigo.LlamarCorutinaEnemigo();
        
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
            MergePanel.SetActive(false);
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
        if(Group1.GetComponent<PlayerStats>().numComponentesGrupo == 0)
        {
            Destroy(Group1.gameObject);
            grid.NodeFromWorldPoint(Group2.transform.position).objectInNode = Group2;
            
            
        }
        else if (Group2.GetComponent<PlayerStats>().numComponentesGrupo == 0)
        {
            Destroy(Group2.gameObject);
            grid.NodeFromWorldPoint(Group1.transform.position).objectInNode = Group1;


        }
    }
    public void EnterCity(GameObject Group)
    {
        UnitsInCity += Group.GetComponent<PlayerStats>().numComponentesGrupo;
        Destroy(Group);
    }
    public void GameOver()
    {
        gAMEoVERpANEL.SetActive(true);
    }
}
