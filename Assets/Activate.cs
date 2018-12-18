using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour
{
    public GameObject grid;
    // Start is called before the first frame update
 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            grid.SetActive(!grid.active);
        }
    }
}
