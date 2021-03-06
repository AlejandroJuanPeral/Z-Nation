﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BottonScript : MonoBehaviour
{
    public GameObject Manager,Group1,Group2,MergePanel;
    public InputField Input;
 
    public void Merge(Slider slider)
    {
        Group1.GetComponent<PlayerStats> ().numComponentesGrupo =(int) slider.value;
        Group2.GetComponent<PlayerStats>().numComponentesGrupo = (int) slider.maxValue - (int) slider.value;
        if (Group1.GetComponent<PlayerStats>().numComponentesGrupo == 0) Destroy(Group1);
        if (Group2.GetComponent<PlayerStats>().numComponentesGrupo == 0) Destroy(Group2);
        MergePanel.SetActive(false);
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
        string aux = Input.text;
        int num = System.Convert.ToInt32(aux);

        Manager.GetComponent<PlayerManager>().NewGroup(num);
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
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
