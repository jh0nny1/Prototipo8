using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class MouseManager : MonoBehaviour {

    public Ray screenPointToRay;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		screenPointToRay = Camera.main.ScreenPointToRay(Input.mousePosition);

    }

    
}
