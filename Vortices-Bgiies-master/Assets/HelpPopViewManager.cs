using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPopViewManager : MonoBehaviour {

    public PopUpController SummaryCanvasHelpPopUp;
    public PopUpController OutpathViewHelpPopUp;
    public PopUpController DioCanvasHelpPopUp;
    public PopUpController VisualizationCanvasHelpPopUp;
    public PopUpController InterfacesCanvasHelpPopUp;
    public PopUpController ActionParingCanvasHelpPopUp;


    [HideInInspector]
    string SummaryCanvasHelpTextDefault = "MOTIONS is a platform that allows experimentation and evaluation with different interfaces and sensors. \n\n On these window you must create an evaluation profile, and select the characteristics of the experiment by clicking the buttons on the left side. When your are finish, this window will display a summary of the most relevant settings of your experiment. \n\n Press Start Evaluations to to begin the evaluation!";
    string OutpathViewHelpTextDefault = "On this window you must select the folder that will contain the output files of your experiments, which includes the actions made by the participant and the captured data by the different sensors you utilized.";
    string DioCanvasHelpTextDefault = "On this window you must select the type of Object Information that you wish to experiment and interact on.";
    string VisualizationCanvasHelpTextDefault = "On this window you must select the way that the Information Objects are going to be displayed, which were previously chosen.";
    string InterfacesCanvasHelpTextDefault = "On this window you must select the sensors that you will utilize on the experiment.";
    string ActionParingCanvasHelpTextDefault = "On this window you must paired the functionalities of the experiment with the actions of the sensor(s) previously chosen. \n\n (*) Some sensors need values of reference to trigger or nullify an action.";

    string textTopHelpView = "Help";

    public void LoadSummaryCanvasHelpPopUp()
    {
        SummaryCanvasHelpPopUp.LaunchPopUpScrolldown(textTopHelpView, SummaryCanvasHelpTextDefault);
    }

    public void LoadOutpathViewHelpPopUp()
    {
        OutpathViewHelpPopUp.LaunchPopUpScrolldown(textTopHelpView, OutpathViewHelpTextDefault);
    }

    public void LoadDioCanvasHelpPopUp()
    {
        DioCanvasHelpPopUp.LaunchPopUpScrolldown(textTopHelpView, DioCanvasHelpTextDefault);
    }

    public void LoadVisualizationCanvasHelpPopUp()
    {
        VisualizationCanvasHelpPopUp.LaunchPopUpScrolldown(textTopHelpView, VisualizationCanvasHelpTextDefault);
    }

    public void LoadInterfacesCanvasHelpPopUp()
    {
        InterfacesCanvasHelpPopUp.LaunchPopUpScrolldown(textTopHelpView, InterfacesCanvasHelpTextDefault);
    }

    public void LoadActionParingCanvasHelpPopUp()
    {
        ActionParingCanvasHelpPopUp.LaunchPopUpScrolldown(textTopHelpView, ActionParingCanvasHelpTextDefault);
    }
}
