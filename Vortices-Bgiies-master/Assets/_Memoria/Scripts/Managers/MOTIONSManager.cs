using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using UnityEngine.SceneManagement;
using Memoria;

public class MOTIONSManager : MonoBehaviour {
    public static MOTIONSManager Instance { set; get; }
    [HideInInspector]
    public List<GameObject> activatedGameObjects;

    [HideInInspector]
    public string[] interfacesWithInputNames;

    [HideInInspector]
    public string[] interfacesNames;

    [HideInInspector]
    public bool informationObjectInitialized, visualizationInitialized;

    [HideInInspector]
    public bool instanced = false;

    CsvCreator currentCsv;

    private void Awake()
    {
        Instance = this;
        instanced = true;
        DontDestroyOnLoad(this);
        GLPlayerPrefs.SetBool("ProfileManagerScope3", "SimulationStarted", false);
        interfacesWithInputNames = new string[]
        {
            "Emotiv",
            "Kinect",
            "NeuroSky",
            "Keyboard",
			"BITalino",
			"TouchScreen",
			"LeapMotion"

        };

        interfacesNames = new string[]
        {
            "Emotiv",
            "EyeTribe",
            "NeuroSky",
            "Kinect",
            "OcculusRift",
            "OpenGlove",
            "LeapMotion",
            "Gamepad",
            "Mouse",
			"Keyboard",
			"BITalino",
			"TouchScreen"

        };
    }


    public void StartEvaluation()
    {
        string scope = ProfileManager.Instance.currentEvaluationScope;
        int scene = GLPlayerPrefs.GetInt(scope, "Scene");
        GLPlayerPrefs.SetInt(scope, "LastUserIDUsed", GLPlayerPrefs.GetInt(ProfileManager.Instance.currentEvaluationScope, "CurrentUserID"));
        informationObjectInitialized = false;
        visualizationInitialized = false;
        //DELETE THIS clean the action mapping list in the action manager, should be triggered by "returning" in the escape-menu
        ActionManager.Instance.updateActionArrayList = new List<System.Action>();
        initializeCsv();

        //Set audio Settings for immersion
        AudioPreSettings audioSettings = new AudioPreSettings();
        audioSettings.AudioConfiguration(scope);

        if (scene == 0)
        {
            SceneManager.LoadScene("TestScenarioA");
        }
        else if (scene == 1) {
            SceneManager.LoadScene("TestScenarioB");
        }
        else
        {
            SceneManager.LoadScene("EmotivTraining");
        }
        //SceneManager.LoadScene("FullScene");
    }

    public void CheckActionManagerInitialization()
    {
		
        if(visualizationInitialized && informationObjectInitialized)
        {
			
            if (ActionManager.Instance.ReloadMappingActions())
            {
                if(ActionManager.Instance.actionsLimitations.AddLimitationToAction(
                    ActionManager.Instance.ActionConditionButtons(KeyCode.O),
                    0))
                {
                    LoadersManager.Instance.LoadInterfaces();
                }
                

            }
            ActionManager.Instance.InitializeManager();
        }
    }

    /// <summary>
    /// Adds line to CSV file named by inputName.
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="action"></param>
    /// <param name="objectId"></param>
    public void AddLines(string inputName, string action, string objectId)
    {
        string subfolder = inputName + " data.csv";
        currentCsv.AddLines(action, objectId, subfolder);
    }

    /// <summary>
    /// Adds line to CSV file named "System"
    /// </summary>
    /// <param name="action"></param>
    /// <param name="objectId"></param>
    public void AddLines(string action, string objectId)
    {
        currentCsv.AddLines(action, objectId, "System data.csv");
    }

    /// <summary>
    /// Adds line to CSV file named filename in "subfolder" folder
    /// </summary>
    public void AddLines(string subfolder, string filename, string action, string objectId)
    {
        string subfoldername = subfolder + "\\"+filename + ".csv";
        currentCsv.AddLines(action, objectId, subfoldername);
    }

    public void initializeCsv()
    {
        string outputPath = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "OutputFolderPath");
        outputPath = outputPath + ProfileManager.Instance.profiles[ProfileManager.Instance.currentProfile] + "\\" + ProfileManager.Instance.evaluations[ProfileManager.Instance.currentEvaluation] + "\\";
        System.IO.Directory.CreateDirectory(outputPath);
        currentCsv = new CsvCreator(outputPath);
    }



    /*
     *                                      IMPORTANT:
     * To maintain data integrity, please always note here the names of the Visualizations, 
     * Objects and Interfaces you're adding, so they DON'T repeat themselves when using keys 
     * to store data in the player preferences (GLPlayerPrefs).
     * 
     ***Visualizations:
     *  -Plane
     *  -Sphere
     * 
     ***Objects:
     *  -PlaneImage
     * 
     ***Interfaces:
     *  -Emotiv
     *  -EyeTribe
     *  -NeuroSky
     *  -Kinect
     *  -OcculusRift
     *  -OpenGlove
     *  -LeapMotion
     *  -Gamepad
     *  -Mouse
     *  -BITalino
     *  -TouchScreen
     * 
     */

    /// <summary>
    /// Plane, Sphere
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void VisualizationNames()
    {
        //this function does nothing, but the description shows the names of the current string key of the visualizations available
    }

    /// <summary>
    /// PlaneImage
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void ObjectsNames()
    {
        //this function does nothing, but the description shows the names of the current string key of the objects available
    }

    /// <summary>
    /// Emotiv, EyeTribe, NeuroSky, Kinect, OcculusRift, OpenGlove, LeapMotion, Gamepad. If asking if an interfaces is being used, they keys are "useEmotiv", "useKinect" and so on.
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void InterfacesNames()
    {
        //this function does nothing, but the description shows the names of the current string key of the interfaces available
    }

    /// <summary>
    /// ProfileManager, ActionManager, DIOManager, ConfigManager
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void ManagersNames()
    {
        
    }

    /// <summary>
    /// CurrentInformationObject, CurrentVisualization, CurrentImmersion, CurrentUserID, OutputFolderPath 
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void ConfigurationNames()
    {
        //this function does nothing, but the description shows the names of the current string key of the most common configurations available
    }
}
