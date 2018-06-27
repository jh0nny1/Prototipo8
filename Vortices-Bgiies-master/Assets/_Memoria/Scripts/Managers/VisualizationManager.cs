using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationManager : GLMonoBehaviour {

    public static VisualizationManager Instance { set; get; }

    public PlaneVisualizationManager planeVisualization;
    public SphereVisualizationManager sphereVisualization;
	public TIIESVisualizationManager tiiesVisualization;


    private void Awake()
    {
        Instance = this;
    }

    public void LoadVisualization()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        string currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");

        InterfaceManager.Instance.OnNewScene();

        /*
         * Sphere visualization configuration
         * 
         * 
         */
        if (currentVisualization.Equals("Sphere"))
        {
            sphereVisualization.gameObject.SetActive(true);
            
        }

		if (currentVisualization.Equals("Plane"))
        {
            planeVisualization.gameObject.SetActive(true);
        }

		if (currentVisualization.Equals ("TIIES")) 
		{
			Debug.Log ("Entre al TIIES.setactive true");
			tiiesVisualization.gameObject.SetActive (true);
		}
    }
}
