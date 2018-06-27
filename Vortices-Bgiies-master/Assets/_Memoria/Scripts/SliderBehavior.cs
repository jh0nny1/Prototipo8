using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehavior : MonoBehaviour {

    public Slider adaptativeSlider;

    void Start()
    {
        if (adaptativeSlider.tag == "AudioSlider") {
            adaptativeSlider.value = PlayerPrefs.GetInt("Auditive Immersion");
        }
        else
        {
            adaptativeSlider.value = PlayerPrefs.GetInt("Visual Immersion");
        }
    }

    public void SaveVisualLevel()
    {
        PlayerPrefs.SetInt("Visual Immersion", (int)adaptativeSlider.value);
    }

    public void SaveAuditiveLevel()
    {
        PlayerPrefs.SetInt("Auditive Immersion", (int)adaptativeSlider.value);
    }
    
}
