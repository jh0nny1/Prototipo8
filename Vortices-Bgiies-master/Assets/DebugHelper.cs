using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour {

    public GameObject summaryCanvas;
    public GameObject[] otherCanvas;

    private void Awake()
    {
        foreach (GameObject obj in otherCanvas)
        {
            obj.SetActive(false);
        }
        summaryCanvas.SetActive(true);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
