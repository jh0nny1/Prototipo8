using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopUpButton : MonoBehaviour {
	public GameObject topBar, contentWindow;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			topBar.SetActive (false);
			contentWindow.SetActive (false);
		} else {
			return;
		}
	}
}
