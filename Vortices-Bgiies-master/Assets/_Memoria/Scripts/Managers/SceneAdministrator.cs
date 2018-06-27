using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAdministrator : MonoBehaviour {
    public static SceneAdministrator Instance { set; get; }
    string active_scene, last_scene;
	// Use this for initialization

	void Start () {
        active_scene = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKey(KeyCode.K))
        {
            ChangeScene("calib_scene");
        }
        if (Input.GetKey(KeyCode.L))
        {
            LoadLastScene();
        }
        */
    }

    public void UpdateActiveScene()
    {
        active_scene = SceneManager.GetActiveScene().name;
    }

    public void ChangeScene(string scene_name)
    {
        last_scene = active_scene;
        SceneManager.LoadScene(scene_name);
        active_scene = scene_name;
        Debug.Log("active scene: " + active_scene);
        Debug.Log("Last scene: " + last_scene);        
    }

    public void LoadLastScene()
    {
        SceneManager.LoadScene(last_scene);        
        string aux;
        aux = last_scene;
        last_scene = active_scene;
        active_scene = aux;
    }
}
