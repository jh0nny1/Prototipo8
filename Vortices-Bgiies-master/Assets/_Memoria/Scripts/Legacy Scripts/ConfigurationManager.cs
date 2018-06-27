using System;
using UnityEngine;
using Gamelogic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.Collections;

public class ConfigurationManager : MonoBehaviour
{
    public string sceneName;
    public GameObject mainMenuPanel;
    public GameObject hardwarePanel;
    public GameObject openGlovePanel;
    public GameObject loadImagePanel;
    public GameObject dataOutputPanel;
    public Button backButton;
    //public GameObject fileBrowser;


    [Header("Main Menu")]
    public Dropdown modeDropDown;
    public Button LoadImages;
    public Button Output;
    public Dropdown testDropdown;
    public Dropdown visualization;

    [Header("Hardware")]
    public Toggle useLeapMotionToggle;
    public Toggle usePitchGrabToggle;
    public Toggle useJoystickToggle;
    public Toggle mouseInput;
    public Toggle kinectInput;

    [Header("Data Output")]
    public InputField dataOutputText;
    public InputField userIdText;


    [Header("Unity Open Glove")]
    public Toggle useUnityOpenGlove;


    [Header("Load Images")]
    public InputField imagesText;
    public InputField folderImageAssetText;
    public InputField fileNameText;
    public InputField groupPathText;
    public Button folderPathButton;

    private string Scope = "Config";

    public void Start()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;
        SetVariables();
    }

    public void Update()
    {

    }
    public void SetVariables()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;
        useLeapMotionToggle.isOn = GLPlayerPrefs.GetBool(Scope, "UseLeapMotion");
        if (useLeapMotionToggle.isOn)
        {
            usePitchGrabToggle.enabled = true;
            usePitchGrabToggle.enabled = true;
            usePitchGrabToggle.isOn = GLPlayerPrefs.GetBool(Scope, "UsePitchGrab");
        }

        if (!(useLeapMotionToggle.isOn && useJoystickToggle.isOn) || !(mouseInput.isOn && kinectInput.isOn))
            useUnityOpenGlove.interactable = false;

        useJoystickToggle.isOn = GLPlayerPrefs.GetBool(Scope, "UseJoystic");

        useUnityOpenGlove.isOn = GLPlayerPrefs.GetBool(Scope, "useUnityOpenGlove");

        kinectInput.isOn = GLPlayerPrefs.GetBool(Scope, "KinectInput");
        mouseInput.isOn = GLPlayerPrefs.GetBool(Scope, "MouseInput");

        if (GLPlayerPrefs.GetBool(Scope, "BGIIESMode"))
            modeDropDown.value = 1;
        else
            modeDropDown.value = 0;

        if (GLPlayerPrefs.GetBool(Scope, "visualizationPlane"))
            visualization.value = 1;
        else
            visualization.value = 0;


        dataOutputText.text = GLPlayerPrefs.GetString(Scope, "DataOutput");


        userIdText.text = GLPlayerPrefs.GetInt(Scope, "UserID").ToString();

        imagesText.text = GLPlayerPrefs.GetString(Scope, "Images");
        folderImageAssetText.text = GLPlayerPrefs.GetString(Scope, "FolderImageAssetText");
        //folderSmallText.text = GLPlayerPrefs.GetString(Scope, "FolderSmallText");
        fileNameText.text = GLPlayerPrefs.GetString(Scope, "FileName");
        groupPathText.text = GLPlayerPrefs.GetString(Scope, "GroupPath");

        testDropdown.value = GLPlayerPrefs.GetInt(Scope, "Test");
    }

    public void StartSimulation()
    {
        UpdateProfileValues();
        SceneManager.LoadScene(sceneName);
    }

    public void UpdateProfileValues()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;
        GLPlayerPrefs.SetBool(Scope, "UseLeapMotion", useLeapMotionToggle.isOn);
        GLPlayerPrefs.SetBool(Scope, "UsePitchGrab", usePitchGrabToggle.isOn);
        GLPlayerPrefs.SetBool(Scope, "UseHapticGlove", useUnityOpenGlove.isOn);
        GLPlayerPrefs.SetBool(Scope, "UseJoystic", useJoystickToggle.isOn);

        GLPlayerPrefs.SetBool(Scope, "MouseInput", mouseInput.isOn);
        GLPlayerPrefs.SetBool(Scope, "KinectInput", kinectInput.isOn);

        if (modeDropDown.value == 0)
            GLPlayerPrefs.SetBool(Scope, "BGIIESMode", false);
        else
            GLPlayerPrefs.SetBool(Scope, "BGIIESMode", true);

        if (visualization.value == 0)
            GLPlayerPrefs.SetBool(Scope, "visualizationPlane", false);
        else
            GLPlayerPrefs.SetBool(Scope, "visualizationPlane", true);

        GLPlayerPrefs.SetString(Scope, "DataOutput", dataOutputText.text);


        GLPlayerPrefs.SetInt(Scope, "UserID", Convert.ToInt32(userIdText.text));

        GLPlayerPrefs.SetString(Scope, "Images", imagesText.text);
        GLPlayerPrefs.SetString(Scope, "FolderImageAssetText", folderImageAssetText.text);
        //GLPlayerPrefs.SetString(Scope, "FolderSmallText", folderSmallText.text);
        GLPlayerPrefs.SetString(Scope, "FileName", fileNameText.text);
        GLPlayerPrefs.SetString(Scope, "GroupPath", groupPathText.text);

        GLPlayerPrefs.SetInt(Scope, "Test", testDropdown.value);
    }

    public void LoadImage()
    {
        mainMenuPanel.SetActive(false);
        backButton.gameObject.SetActive(true);

        loadImagePanel.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void DataOutput()
    {
        mainMenuPanel.SetActive(false);

        dataOutputPanel.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void Back()
    {
        mainMenuPanel.SetActive(true);

        loadImagePanel.SetActive(false);
        dataOutputPanel.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
    
    public void UseLeapMotionToggle()
    {
        if (useLeapMotionToggle.isOn)
        {
            if (!usePitchGrabToggle.gameObject.activeSelf)
                usePitchGrabToggle.gameObject.SetActive(true);

            useJoystickToggle.isOn = false;
            useJoystickToggle.interactable = false;
            useUnityOpenGlove.interactable = true;
        }
        else
        {
            usePitchGrabToggle.gameObject.SetActive(false);
            useJoystickToggle.interactable = true;
            useUnityOpenGlove.interactable = false;
        }
    }

    public void UseJoystickToggle()
    {
        if (useJoystickToggle.isOn)
        {
            useLeapMotionToggle.isOn = false;
            useLeapMotionToggle.interactable = false;

            useUnityOpenGlove.isOn = false;
            useUnityOpenGlove.interactable = false;
        }
        else
            useLeapMotionToggle.interactable = true;
    }

    public void KinectInputToggle()
    {
        if (kinectInput.isOn)
        {
            mouseInput.isOn = false;
            mouseInput.interactable = false;
            useUnityOpenGlove.interactable = true;
        }
        else
        {
            mouseInput.interactable = true;
            useUnityOpenGlove.isOn = false;
            useUnityOpenGlove.interactable = false;
        }
    }

    public void MouseInputToggle()
    {
        if (mouseInput.isOn)
        {
            kinectInput.isOn = false;
            kinectInput.interactable = false;
        }
        else
            kinectInput.interactable = true;
    }

    public void PinchGrabToggle()
    {
        if (usePitchGrabToggle.isOn)
            useUnityOpenGlove.interactable = false;
        else
        {
            useUnityOpenGlove.interactable = true;
            useUnityOpenGlove.isOn = false;
        }
    }

    public void OpenGloveToggle()
    {
        if (useUnityOpenGlove.isOn && useLeapMotionToggle.isOn)
            usePitchGrabToggle.interactable = false;
        else
        {
            usePitchGrabToggle.interactable = true;
            usePitchGrabToggle.isOn = false;
        }
    }
    public void ModeDropDownChange()
    {
        if (modeDropDown.value == 0)
        {
            mouseInput.isOn = false;
            kinectInput.isOn = false;

            mouseInput.gameObject.SetActive(false);
            kinectInput.gameObject.SetActive(false);

            useLeapMotionToggle.gameObject.SetActive(true);
            useJoystickToggle.gameObject.SetActive(true);
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "BGIIESMode", false);
        }
        else
        {
            useLeapMotionToggle.isOn = false;
            useJoystickToggle.isOn = false;

            useLeapMotionToggle.gameObject.SetActive(false);
            useJoystickToggle.gameObject.SetActive(false);
            usePitchGrabToggle.gameObject.SetActive(false);

            kinectInput.gameObject.SetActive(true);
            mouseInput.gameObject.SetActive(true);
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "BGIIESMode", true);
        }
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void ExaminarFolderPath()
    {
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        FileBrowser.ShowLoadDialog(null, null, true, null, "Load", "Select");
        StartCoroutine(ObtenerGroupFolder());
    }
    IEnumerator ObtenerGroupFolder()
    {
        yield return FileBrowser.WaitForLoadDialog(true, null, "Load File", "Load");
        if (FileBrowser.Result != null)
            folderImageAssetText.text = FileBrowser.Result + "\\";

    }

    public void ExaminarGroupPath()
    {
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Text Files", ".csv"));
        FileBrowser.SetDefaultFilter(".csv");
        FileBrowser.ShowLoadDialog(null, null, false, null, "Load", "Select");
        StartCoroutine(ShowResultCoroutine());
    }

    IEnumerator ShowResultCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        if(FileBrowser.Result != null)
            groupPathText.text = FileBrowser.Result;
    }

    public void ObtenerOutputPath()
    {
        FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Text Files", ".csv"));
        FileBrowser.SetDefaultFilter(".csv");
        FileBrowser.ShowLoadDialog(null, null, false, null, "Load", "Select");
        StartCoroutine(GuardarOutputPath());
    }

    IEnumerator GuardarOutputPath()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        if (FileBrowser.Result != null)
            dataOutputText.text = FileBrowser.Result;
    }
}