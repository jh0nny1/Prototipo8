using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class LoadersManager : MonoBehaviour {

    public static LoadersManager Instance { set; get; }

    public GameObject[] interfacesWithInput;
    /*
     * The game objects in the interfaces with input variable must be the loaders of the corresponding interfaces in the same order
     *      the appear in the MOTIONSManager.
     * For example, as you can see the MOTIONSManager variable interfacesWithInputNames lists Emotiv, Kinect, NeuroSky and Keyboard, so the
     *      first element of the interfacesWithInput gameobject array must be the EmotivMappingLoader, then the second element must be the KinectMappingLoader and so on.
     */

    private void Awake()
    {
        Instance = this;
    }

    public void LoadInterfaces()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        for(int i = 0; i < MOTIONSManager.Instance.interfacesWithInputNames.Length; i++)
        {
			if (GLPlayerPrefs.GetBool(Scope, "use" + MOTIONSManager.Instance.interfacesWithInputNames[i]))
            {
				
                interfacesWithInput[i].SetActive(true);
				;
            }

        }        
    }
}
