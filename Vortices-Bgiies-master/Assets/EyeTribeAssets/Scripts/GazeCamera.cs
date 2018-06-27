/*
 * Copyright (c) 2013-present, The Eye Tribe. 
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the LICENSE file in the root directory of this source tree. 
 *
 */

using UnityEngine;
using TETCSharpClient;
using TETCSharpClient.Data;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using Gamelogic;
using Memoria;

/// <summary>
/// Component attached to 'Main Camera' of '/Scenes/std_scene.unity'.
/// This script handles the navigation of the 'Main Camera' according to 
/// the GazeData stream recieved by the EyeTribe Server.
/// </summary>
public class GazeCamera : MonoBehaviour, IGazeListener
{
    public static GazeCamera Instance { set; get; }
    public string dataLogPath;
    public CsvCreator csvCreator;
    private string Scope = "Vortices2Config";


    public GameObject uvplane;

    public bool useGazeTracker = true;

    private Camera cam;

    private Component gazeIndicator;

    private Collider currentHit;

    private GazeDataValidator gazeUtils;

    private Vector3 screenPoint;


    public void Awake()
    {
        Instance = this;        
    }

    public void Initialize()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;
        cam = Camera.main;
        transform.SetParent(cam.transform);
        if (useGazeTracker)
        {            
            GameObject go = Instantiate(uvplane, Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.SetParent(Camera.main.transform);
            go.transform.position = Vector3.zero;
            gazeIndicator = go.transform;
        }

        //initialising GazeData stabilizer
        gazeUtils = new GazeDataValidator(30);

        //activate C# TET client, default port
        GazeManager.Instance.Activate
        (
            GazeManager.ApiVersion.VERSION_1_0,
            GazeManager.ClientMode.Push
        );

        //register for gaze updates
        GazeManager.Instance.AddGazeListener(this);
        /*
         * Initializes the CvsCreator to store data in a log
         */
        dataLogPath = GLPlayerPrefs.GetString(Scope, "TheEyeTribeDataPath");
        Debug.Log("eyetribe path: " + GLPlayerPrefs.GetString(Scope, "TheEyeTribeDataPath"));
        if (dataLogPath.Equals(""))
        {
            csvCreator = new CsvCreator("EyeTribeDataLog\\data.csv");
        }
        else
        {
            csvCreator = new CsvCreator(dataLogPath);
        }
    }

    public void ChangeGazeTrackerStatus()
    {
        useGazeTracker = !useGazeTracker;
    }

    public void OnGazeUpdate(GazeData gazeData)
    {
        //Add frame to GazeData cache handler
        gazeUtils.Update(gazeData);
        /*
         * Log value recording on each update
         */
        csvCreator.AddLines("Time Stamp: "+gazeData.TimeStampString, "");
        csvCreator.AddLines("Is fixated? " + gazeData.IsFixated.ToString(), "");
        csvCreator.AddLines("State: " + gazeData.State.ToString(), "");
        csvCreator.AddLines("Raw X & Y: " + gazeData.RawCoordinates.X.ToString() + " , " + gazeData.RawCoordinates.Y.ToString(), "");
        csvCreator.AddLines("Smoothed: " + gazeData.SmoothedCoordinates.X.ToString() + " , " + gazeData.SmoothedCoordinates.Y.ToString(), "");

        csvCreator.AddLines("Left Eye Raw: " + gazeData.LeftEye.RawCoordinates.X.ToString() + ", " + gazeData.LeftEye.RawCoordinates.Y.ToString(), "");
        csvCreator.AddLines("Left Eye Smooth: " + gazeData.LeftEye.SmoothedCoordinates.X.ToString() + ", " + gazeData.LeftEye.SmoothedCoordinates.Y.ToString(), "");
        csvCreator.AddLines("Left Eye Pupil Size: " + gazeData.LeftEye.PupilSize.ToString() , "");
        csvCreator.AddLines("Left Eye Pupil Center: " + gazeData.LeftEye.PupilCenterCoordinates.X.ToString() + ", " + gazeData.LeftEye.PupilCenterCoordinates.Y.ToString(), "");

        csvCreator.AddLines("Right Eye Raw: " + gazeData.RightEye.RawCoordinates.X.ToString() + ", " + gazeData.RightEye.RawCoordinates.Y.ToString(), "");
        csvCreator.AddLines("Right Eye Smooth: " + gazeData.RightEye.SmoothedCoordinates.X.ToString() + ", " + gazeData.RightEye.SmoothedCoordinates.Y.ToString(), "");
        csvCreator.AddLines("Right Eye Pupil Size: " + gazeData.RightEye.PupilSize.ToString(), "");
        csvCreator.AddLines("Right Eye Pupil Center: " + gazeData.RightEye.PupilCenterCoordinates.X.ToString() + ", " + gazeData.RightEye.PupilCenterCoordinates.Y.ToString(), "");
    }

    public void UpdateGazeCamera()
    {
        Point2D gazeCoords = gazeUtils.GetLastValidSmoothedGazeCoordinates();
        if (null != gazeCoords)
        {
            //map gaze indicator
            Point2D gp = UnityGazeUtils.getGazeCoordsToUnityWindowCoords(gazeCoords);
            
            screenPoint = new Vector3((float)gp.X, (float)gp.Y, cam.nearClipPlane + .1f);

            if (useGazeTracker)
            {
                Vector3 planeCoord = cam.ScreenToWorldPoint(screenPoint);
                gazeIndicator.transform.position = planeCoord;
            }
            
            //handle collision detection, just as an example
            //checkGazeCollision(screenPoint);
        }

        
    }

    public Vector3 getScreenPoint()
    {
        return screenPoint;
    }

    private void checkGazeCollision(Vector3 screenPoint)
    {
        Ray collisionRay = cam.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(collisionRay, out hit))
        {
           //The hit variable will return the information of anything the raycast hits.
        }
    }

    void OnGUI()
    {
        
    }

    void OnApplicationQuit()
    {
        GazeManager.Instance.RemoveGazeListener(this);
    }
}
