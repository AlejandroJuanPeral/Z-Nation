using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    public Slider slider;
    public Text SText;

    
    // Update is called once per frame
    void Update()
    {
        SText.text = (slider.maxValue - slider.value) + "					" + slider.value;
    }
}
