using System.Collections;
using System.Collections.Generic;
using Gamelogic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour {

    public static ProfileManager Instance { set; get; }
    [HideInInspector]
    public string currentEvaluationScope;
    [HideInInspector]
    public string profileScope;
    
    [HideInInspector]
    public int currentProfile;
    [HideInInspector]
    public int currentEvaluation;
    [HideInInspector]
    public string[] profiles;
    [HideInInspector]
    public string[] evaluations;


    string profileManagerScope = "ProfileManager3";
    // Use this for initialization
    public void Awake () {
        Instance = this;
        DontDestroyOnLoad(this);
        //If there are no profiles found in the systems registry, it creates a new array of names. This is because there is no default value for arrays, but there are for all other data types used.
        if(!GLPlayerPrefs.GetBool(profileManagerScope, "RegistryFound")){
            string[] aux = new string[1];
            aux[0] = "Default Profile";
            GLPlayerPrefs.SetStringArray(profileManagerScope, "ProfileNamesList", aux);
            GLPlayerPrefs.SetBool(profileManagerScope, "RegistryFound", true);
            aux = new string[1];
            aux[0] = "Default Evaluation";
            GLPlayerPrefs.SetStringArray("Default Profile", "EvaluationNamesList", aux);
        }
        profiles = GLPlayerPrefs.GetStringArray(profileManagerScope,"ProfileNamesList");
        currentProfile = GLPlayerPrefs.GetInt(profileManagerScope, "CurrentProfile");
        profileScope = profiles[currentProfile];
        currentEvaluation = GLPlayerPrefs.GetInt(profileScope, "CurrentEvaluation");
		evaluations = GLPlayerPrefs.GetStringArray(profileScope, "EvaluationNamesList");
		currentEvaluationScope = profileScope + evaluations[currentEvaluation];
		MOTIONSManager.Instance.initializeCsv();
    }

    public bool UpdateCurrentProfile(int lastProfileUsedNumber)
    {
        currentProfile = lastProfileUsedNumber;
        GLPlayerPrefs.SetInt(profileManagerScope, "CurrentProfile", currentProfile);
        profileScope = profiles[currentProfile];
        currentEvaluation = GLPlayerPrefs.GetInt(profileScope, "CurrentEvaluation");
        evaluations = GLPlayerPrefs.GetStringArray(profileScope, "EvaluationNamesList");
        currentEvaluationScope = profileScope + evaluations[currentEvaluation];
        return true;
    }

    public bool UpdateCurrentEvaluation(int lastEvaluationUsedNumber)
    {
        currentEvaluation = lastEvaluationUsedNumber;
        GLPlayerPrefs.SetInt(profileScope, "CurrentEvaluation", currentEvaluation);
        currentEvaluationScope = profileScope + evaluations[currentEvaluation];
        return true;
    }

    public bool AddNewProfile(string newProfile)
    {
        if (CheckRepeatedProfileName(newProfile))
        {
            string[] aux = new string[profiles.Length];
            profiles.CopyTo(aux, 0);
            int newLength = profiles.Length+1;
            profiles = new string[newLength];
            aux.CopyTo(profiles, 0);
            profiles[newLength - 1] = newProfile;
            GLPlayerPrefs.SetStringArray(profileManagerScope, "ProfileNamesList", profiles);
            aux = new string[1];
            aux[0] = "Default Evaluation";
            GLPlayerPrefs.SetStringArray(newProfile, "EvaluationNamesList", aux);
            UpdateCurrentProfile(newLength - 1);            
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckRepeatedProfileName(string newProfile)
    {
        foreach(string s in profiles)
        {
            if (newProfile.Equals(s))
                return false;
        }
        return true;
    }

    public bool AddNewEvaluation(string newEvaluation)
    {
        if (CheckRepeatedEvaluationName(newEvaluation))
        {
            string[] aux = new string[evaluations.Length];
            evaluations.CopyTo(aux, 0);
            int newLength = evaluations.Length + 1;
            evaluations = new string[newLength];
            aux.CopyTo(evaluations, 0);
            evaluations[newLength - 1] = newEvaluation;
            GLPlayerPrefs.SetStringArray(profileScope, "EvaluationNamesList", evaluations);
            UpdateCurrentEvaluation(newLength - 1);
            SetEvaluationDefaultValues();
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckRepeatedEvaluationName(string newEvaluation)
    {
        foreach (string s in evaluations)
        {
            if (newEvaluation.Equals(s))
                return false;
        }
        return true;
    }

    void SetEvaluationDefaultValues()
    {
        GLPlayerPrefs.SetString(currentEvaluationScope, "CurrentVisualization", "Sphere");
        GLPlayerPrefs.SetString(currentEvaluationScope, "CurrentInformationObject", "PlaneImage");
    }


}
