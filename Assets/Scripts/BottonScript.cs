using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottonScript : MonoBehaviour
{
    public GameObject Manager;
    public InputField Input;
 
    public void Merge(Slider slider, GameObject Group1, GameObject Group2)
    {
        Group1.GetComponent< PlayerStats > ().numComponentesGrupo = slider.value;
        Group2.GetComponent<PlayerStats>().numComponentesGrupo = slider.maxValue - slider.value;

    }
    public void SeparatePanel()
    {
        Manager.GetComponent<PlayerManager>().Separate();

    }
    public void Separate(Slider slider)
    {
        Manager.GetComponent<PlayerManager>().SeparateNewGroup(slider.maxValue - slider.value);

    }
    public void newGroup()
    {

        //Manager.GetComponent<PlayerManager>().NewGroup((int)Input.text);

    }

    public void EndTurn()
    {
        Manager.GetComponent<PlayerManager>().FinishTurn();

    }
    public void Next()
    {
        Manager.GetComponent<PlayerManager>().NextGroup();
    }
    public void CreateUnit(Text unitText)
    {
        Manager.GetComponent<PlayerManager>().UpdateBarracon();

    }
    public void UpdateBarracon(Text BarText)
    {
        Manager.GetComponent<PlayerManager>().UpdateBarracon();

    }

}
