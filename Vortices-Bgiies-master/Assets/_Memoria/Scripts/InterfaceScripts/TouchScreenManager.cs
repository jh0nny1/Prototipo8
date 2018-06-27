using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class TouchScreenManager : MonoBehaviour {

	public Ray screenPointToRay;
	public bool[] GestureActive;
	Vector2 firstTouchposs;
	Vector2 secondTouchposs;
	Vector2 currentSwipe;
	float minSwipeLenght = 200f;
	float previus_distance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Estoy en el update del TouchScreenManager");
		/*
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches) {
				screenPointToRay = Camera.main.ScreenPointToRay (touch.position);
			}
		
		}
		*/
		/*
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				screenPointToRay = Camera.main.ScreenPointToRay (touch.position);
			}
				
		}
		*/

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				Debug.Log ("Entre al touchphase began");
				screenPointToRay = Camera.main.ScreenPointToRay (touch.position);
			} else {
				Debug.Log ("Estoy en la phase de movimiento");
			}

		}

		if (Input.touchCount > 0) {

			Touch t = Input.GetTouch (0);

			if (t.phase == TouchPhase.Began) {
				firstTouchposs = new Vector2 (t.position.x, t.position.y);
			}

			if (t.phase == TouchPhase.Ended) {

				secondTouchposs = new Vector2 (t.position.x, t.position.y);
				currentSwipe = new Vector2 (secondTouchposs.x - firstTouchposs.x, secondTouchposs.y - firstTouchposs.y);

				if (currentSwipe.magnitude < minSwipeLenght) {
					//Swape_text.text = "Swape corto";
					return;
				}

				currentSwipe.Normalize ();

				if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					GestureActive [0] = true;
					//Swape_text.text = "Swape arriba";
				} else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					GestureActive [1] = true;
					//Swape_text.text = "Swape abajo";
				} else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					GestureActive [3] = true;
					//Swape_text.text = "Swape izquierda";
				} else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					GestureActive [2] = true;
					//Swape_text.text = "Swape derecha";
				}
			}

		}

		if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)) {
			previus_distance = Vector2.Distance (Input.GetTouch (0).position, Input.GetTouch (1).position);

		}

		else if(Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)){
			float distance;
			Vector2 touch1 = Input.GetTouch (0).position;
			Vector2 touch2 = Input.GetTouch (1).position;

			distance = Vector2.Distance (touch1, touch2);

			float pinch = previus_distance - distance;


			previus_distance = distance;



			if (pinch > 5) {
				GestureActive [4] = true;
				//Swape_text.text = "Zoom_in";

			} else if (pinch < -5) {
				GestureActive [5] = true; 
				//Swape_text.text = "Zoom_out";

			}

		}

		





		//Debug.Log ("Estoy en el update del TouchScreenManager");
		//if (Input.touchCount > 0) {
		//	screenPointToRay = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
		
		//}

		//var Ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);

		
	}

	private void OnEnable()
	{
	
	}
}
