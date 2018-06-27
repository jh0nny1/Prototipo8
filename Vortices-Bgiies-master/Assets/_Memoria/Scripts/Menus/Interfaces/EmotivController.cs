using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotivController : MonoBehaviour {

    public InterfacesController interfacesInstance;
    public GameObject emotivConfigCanvas, emotivTrainingCanvas;

    public void ConfigurationButtons()
    {
        interfacesInstance.scrolldownButtons.AddButtonToList(ShowLogin,"Login to EMOTIV");
        interfacesInstance.scrolldownButtons.AddButtonToList(ShowTraining,"Train mental commands");
        interfacesInstance.scrolldownButtons.ShowButtonList("Emotiv Configuration");
    }

    public void ShowLogin()
    {
        InterfaceManager.Instance.eegManager.StartEmotivInsight();
        emotivConfigCanvas.SetActive(true);
    }

    public void ShowTraining()
    {
        InterfaceManager.Instance.eegManager.StartEmotivInsight();
        interfacesInstance.gameObject.SetActive(false);
        emotivTrainingCanvas.SetActive(true);
    }
}
