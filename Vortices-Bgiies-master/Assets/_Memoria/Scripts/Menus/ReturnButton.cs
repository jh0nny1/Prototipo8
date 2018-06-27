using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour {
	public GameObject resumeCanvas, currentCanvas;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			resumeCanvas.SetActive (true);
			currentCanvas.SetActive (false);
		} else {
			return;
		}
	}
}
