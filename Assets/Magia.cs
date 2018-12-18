using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magia : MonoBehaviour
{
    public List<GameObject> spawnResources;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            spawnResources[index].SetActive(true);
            index++;
            if (index >= spawnResources.Count -1)
            {
                Destroy(this);
            }
        }
    }
}
