using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class EventSystemChecker : MonoBehaviour
{
    [HideInInspector]
    public static EventSystemChecker Instance { set; get; }
    //public GameObject eventSystem;
    public GameObject eventSystem;
    [HideInInspector]
    public bool originalEventSystemExist = false;
    private GameObject obj;
	// Use this for initialization
	void Awake ()
	{
        Instance = this;
	    if(!FindObjectOfType<EventSystem>())
        {
           //Instantiate(eventSystem);
            obj = new GameObject("EventSystem");
            obj.AddComponent<EventSystem>();
            obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;            
        }
        else
        {
            //eventSystem[0].GetComponent<LeapInputModule>().
            eventSystem.SetActive(false);
            obj = new GameObject("EventSystem");
            obj.AddComponent<EventSystem>();
            obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
            obj.SetActive(false);
            eventSystem.SetActive(true);
            originalEventSystemExist = true;
        }
	}

    public void ActivateOrigianlEventSystem()
    {
        obj.SetActive(false);
        eventSystem.SetActive(true);
    }

    public void ActivateSecondaryEventSystem()
    {
        eventSystem.SetActive(false);
        obj.SetActive(true);
    }



}
