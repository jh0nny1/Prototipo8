using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaderTimer : MonoBehaviour {

    public Toggle OpenMenuButton;
    public Text text;
    float timer = 8f;
    float startTime = 0f;
    int aux;
    // Use this for initialization
    void OnEnable () {
        startTime = Time.unscaledTime;
    }

    void Update()
    {
        float elapsedTime = Time.unscaledTime - startTime;
        timer = 9f - elapsedTime;
        aux = (int)timer;
        text.text = "Please, hold that thought for " + aux.ToString() + " seconds";
        if(timer < 1)
            gameObject.SetActive(false);
    }



}
