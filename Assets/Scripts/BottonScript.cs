using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottonScript : MonoBehaviour
{
    public Slider slider;
    public GameObject Manager;
    public void OpenMerge(GameObject Group1, GameObject Group2)
    {

    }
    public void Merge(Slider slider)
    {

    }
    public void Separate(GameObject Group1)
    {

    }
    public void newGroup(InputField input)
    {
        
    }
    public void CreateUniy(Transform pos)
    {

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
