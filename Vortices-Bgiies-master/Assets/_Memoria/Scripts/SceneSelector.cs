using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelector : MonoBehaviour {
    public Dropdown dropdown;

    void Start()
    {
        dropdown.value = PlayerPrefs.GetInt("Scene") - 1;
    }

    public void ChangeSceneSelected()
    {
        PlayerPrefs.SetInt("Scene", dropdown.value + 1);
    }
}
