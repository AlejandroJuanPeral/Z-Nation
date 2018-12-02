using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject FinishButton, CityPanel, RecoursesPanel, MergePanel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayerTurn()
    {
        FinishButton.SetActive(true);

    }
    void EnemyTurn()
    {
        FinishButton.SetActive(false);

    }
}
