using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {


    public void LoadByPlayerPref()
    {
        //SceneManager.LoadScene(PlayerPrefs.GetInt("Scene"));
        SceneManager.LoadScene("TestScenarioA");
    }
    public void LoadByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}
