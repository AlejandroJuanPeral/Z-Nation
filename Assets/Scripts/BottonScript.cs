using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottonScript : MonoBehaviour
{
    public GameObject Manager,Group1,Group2;
    public InputField Input;
 
    public void Merge(Slider slider)
    {
        Group1.GetComponent<PlayerStats> ().numComponentesGrupo =(int) slider.value;
        Group2.GetComponent<PlayerStats>().numComponentesGrupo = (int) slider.maxValue - (int) slider.value;
        if (Group1.GetComponent<PlayerStats>().numComponentesGrupo == 0) Destroy(Group1);
        if (Group2.GetComponent<PlayerStats>().numComponentesGrupo == 0) Destroy(Group2);
    }
    public void SeparatePanel()
    {
        Manager.GetComponent<PlayerManager>().Separate();

    }
    public void Separate(Slider slider)
    {
        Manager.GetComponent<PlayerManager>().SeparateNewGroup((int)(slider.maxValue - slider.value));
    }
    public void newGroup()
    {
        Manager.GetComponent<PlayerManager>().NewGroup(System.Convert.ToInt32(Input.text));
    }

    public void EndTurn()
    {
        Manager.GetComponent<PlayerManager>().FinishTurn();

    }
    public void Next()
    {
        Manager.GetComponent<PlayerManager>().NextGroup();
    }
    public void CreateUnit()
    {
        Manager.GetComponent<PlayerManager>().NewUnit();

    }
    public void UpdateBarracon()
    {
        Manager.GetComponent<PlayerManager>().UpdateBarracon();

    }

}
