using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour {
    private bool isGreen = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            	
	}

    public void ChangeGreen()
    {
        if (!isGreen)
        {
            GetComponent<Image>().color = Color.green;
            isGreen = !isGreen;
        }
    }

    public void ChangeRed()
    {
        if (isGreen) { 
            GetComponent<Image>().color = Color.red;
            isGreen = !isGreen;
        }
    }

}
