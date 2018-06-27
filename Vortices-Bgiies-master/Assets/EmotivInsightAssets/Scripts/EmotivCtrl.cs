using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Emotiv;
using Memoria;
using System;
using Gamelogic;

public class EmotivCtrl : MonoBehaviour {

    public string dataLogPath;
    public InputField userSaveDataPath, userLoadDataPath, userOfflineID;
    string profileNameForSavingUser, profileNameForLoadingUser;
    public Text statusOfflineText;
    int state;
    uint userOfflineStoredID;
    public GameObject modal;
	public InputField userName;
	public InputField password;
	public InputField profileName;
    public bool use_giro_as_camera = false;

	public static EmoEngine engine;
	public static int engineUserID = -1;
	public static int userCloudID = 0;
	static int version	= -1;

    public Action<string> textOutputFunction;

    string Scope;

    string inputName = "EmotivInsight";

    private int currentPersonId;

    /*
	 * Create instance of EmoEngine and set up his handlers for 
	 * user events, connection events and mental command training events.
	 * Init the connection
	*/
    public void Awake()
    {

    }

	public void StartEmotivInsight () 
	{
        engine = EmoEngine.Instance;
        Scope = ProfileManager.Instance.currentEvaluationScope;
        engine.MentalCommandTrainingStarted += new EmoEngine.MentalCommandTrainingStartedEventEventHandler (TrainingStarted);
		engine.MentalCommandTrainingSucceeded += new EmoEngine.MentalCommandTrainingSucceededEventHandler (TrainingSucceeded);
		engine.MentalCommandTrainingCompleted += new EmoEngine.MentalCommandTrainingCompletedEventHandler (TrainingCompleted);
		engine.MentalCommandTrainingRejected += new EmoEngine.MentalCommandTrainingRejectedEventHandler (TrainingRejected);
		engine.MentalCommandTrainingReset += new EmoEngine.MentalCommandTrainingResetEventHandler (TrainingReset);
        
        //If using VORTICES
        engine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(OnEmoStateUpdatedVORTICES);

        /*
         * Event handlers of the example to load and save user data with EMOTIV account, see coment wall at the bottom of the code
         */
        engine.UserAdded += new EmoEngine.UserAddedEventHandler(UserAddedEvent);
        engine.UserRemoved += new EmoEngine.UserRemovedEventHandler(UserRemovedEvent);
        engine.EmoEngineConnected += new EmoEngine.EmoEngineConnectedEventHandler(EmotivConnected);
        engine.EmoEngineDisconnected += new EmoEngine.EmoEngineDisconnectedEventHandler(EmotivDisconnected);


        engine.Connect ();
        
	}

	/*
	 * Init the user, password and profile name if you want it
     * Used as part of the example to load and save user data with an EMOTIV account, see comment wall at the bottom of the code
     */





        /*
         * Call the ProcessEvents() method in Update once per frame
        */
    public void UpdateEmotivInsight () {
		engine.ProcessEvents ();
       
	}

	/*
	 * Close the connection on application exit
	*/
	void OnApplicationQuit() {
		Debug.Log("Application ending after " + Time.time + " seconds");
		engine.Disconnect();
	}

    #region Training Commands
    /*
	 * Several methods for handling the EmoEngine events.
	 * They are self explanatory.
	*/

    public void SetTraining(EdkDll.IEE_MentalCommandAction_t training)
    {
        engine.MentalCommandSetTrainingAction((uint)engineUserID, training);
        //engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
    }

    public void StartTraining()
    {
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
    }

    public void RejectTraining()
    {
        EdkDll.IEE_MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_REJECT);
    }

    public void EraseTraining()
    {
        EdkDll.IEE_MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_ERASE);
    }

    public void NoneTrainingControlCommand()
    {
        EdkDll.IEE_MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_NONE);
    }


    public void TrainingStarted(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Trainig started");        
    }

    public void TrainingCompleted(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Training completed!!");
        MOTIONSManager.Instance.AddLines(inputName,"Training completed", "");
        //engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_ACCEPT);
    }

    public void TrainingRejected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Trainig rejected");
        MOTIONSManager.Instance.AddLines(inputName,"Training rejected", "");
    }

    public void TrainingSucceeded(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Training Succeeded!!");
        engine.MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_ACCEPT);
        MOTIONSManager.Instance.AddLines(inputName,"Training succeeded", "");
    }

    public void TrainingReset(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Command reseted");
        MOTIONSManager.Instance.AddLines(inputName, "Training reseted", "");
    }

    #endregion

    public void Close(){
        MOTIONSManager.Instance.AddLines(inputName,"Application closed", "");
        Application.Quit ();
	}

    /*
 * This method handle the EmoEngine update event, 
 * if the EmoState has the PUSH action, it does "something"
 * The same example could be changed to trigger different actions for all 15 mental commands.
*/
    void OnEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
    {
        EmoState es = e.emoState;
        if (e.userId != 0)
            return;
        Debug.Log("Corrent action: " + es.MentalCommandGetCurrentAction().ToString());
        if (es.MentalCommandGetCurrentAction() == EdkDll.IEE_MentalCommandAction_t.MC_PUSH)
        {
            /*
             *An action or something is triggered 
             */
            Debug.Log("Push command detected");
        }

    }

    void OnEmoStateUpdatedVORTICES(object sender, EmoStateUpdatedEventArgs e)
    {
        EEGManager.Instance.MentalCommandCurrentAction = e.emoState.MentalCommandGetCurrentAction();
        ActionManager.Instance.EmoStateUpdate();
        EEGManager.Instance.MentalCommandCurrentActionPower = e.emoState.MentalCommandGetCurrentActionPower();
        EEGManager.Instance.FacialExpressionIsRightEyeWinking = e.emoState.FacialExpressionIsRightWink();
        EEGManager.Instance.FacialExpressionIsLeftEyeWinking = e.emoState.FacialExpressionIsLeftWink();
        EEGManager.Instance.FacialExpressionIsUserBlinking = e.emoState.FacialExpressionIsBlink();
        EEGManager.Instance.FacialExpressionUpperFaceActionPower = e.emoState.FacialExpressionGetUpperFaceActionPower();
        EEGManager.Instance.FacialExpressionSmileExtent = e.emoState.FacialExpressionGetSmileExtent();
        EEGManager.Instance.FacialExpressionLowerFaceActionPower = e.emoState.FacialExpressionGetLowerFaceActionPower();
        EEGManager.Instance.FacialExpressionLowerFaceAction = e.emoState.FacialExpressionGetLowerFaceAction();
        EEGManager.Instance.FacialExpressionUpperFaceAction = e.emoState.FacialExpressionGetUpperFaceAction();
        //All actions below are for the Log
        MOTIONSManager.Instance.AddLines(inputName, "Clench Extent: " + e.emoState.FacialExpressionGetClenchExtent().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Eyebrow Extent: " + e.emoState.FacialExpressionGetEyebrowExtent().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Lower Face Action: " + e.emoState.FacialExpressionGetLowerFaceAction().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Lower Face Action Power:  " + e.emoState.FacialExpressionGetLowerFaceActionPower().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Upper Face Action: " + e.emoState.FacialExpressionGetUpperFaceAction().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Upper Face Action Power: " + e.emoState.FacialExpressionGetUpperFaceActionPower().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Smile Extent: " + e.emoState.FacialExpressionGetSmileExtent().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Time since start: " + e.emoState.GetTimeFromStart().ToString(), "Time");
        MOTIONSManager.Instance.AddLines(inputName, " Current Action: " + e.emoState.MentalCommandGetCurrentAction().ToString(), "Mental Command");
        MOTIONSManager.Instance.AddLines(inputName, " Current Action Power: " + e.emoState.MentalCommandGetCurrentActionPower().ToString(), "Mental Command");
        MOTIONSManager.Instance.AddLines(inputName, " Is blinking? " + e.emoState.FacialExpressionIsBlink().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Are eyes open? " + e.emoState.FacialExpressionIsEyesOpen().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Is left winking? " + e.emoState.FacialExpressionIsLeftWink().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Is right winking? " + e.emoState.FacialExpressionIsRightWink().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Is looking down? " + e.emoState.FacialExpressionIsLookingDown().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Is looking left? " + e.emoState.FacialExpressionIsLookingLeft().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Is looking right? " + e.emoState.FacialExpressionIsLookingRight().ToString(), "Facial Expression");
        MOTIONSManager.Instance.AddLines(inputName, " Is looking up? " + e.emoState.FacialExpressionIsLookingUp().ToString(), "Facial Expression");
    }

    public void GetActiveActions()
    {
        uint activeactions;
        EdkDll.IEE_MentalCommandGetActiveActions((uint)engineUserID, out activeactions);
        Debug.Log("Active actions: " + activeactions.ToString());
    }

    public void AddActiveCommand(EdkDll.IEE_MentalCommandAction_t command)
    {
        EdkDll.IEE_MentalCommandSetActiveActions((uint)engineUserID, (ulong)command);
        //GetActiveActions();
    }

    public void ResetTraining(EdkDll.IEE_MentalCommandAction_t command)
    {
        EdkDll.IEE_MentalCommandSetTrainingAction((uint)engineUserID, command);
        EdkDll.IEE_MentalCommandSetTrainingControl((uint)engineUserID, EdkDll.IEE_MentalCommandTrainingControl_t.MC_RESET);
    }

    

    #region Connection Cloud
    /* This functions are to connect the to the web services of EMOTIV and load/save already recorded paters. It's left here as an example copied from the original example in 
     * the Emotiv SDK. It's comented because it serves no purpose if there are going to be many users going through the system at the fastest rate possible, as creating an account, validating it,
     * logging in and then saving and loading profile would take too much time.
     * 
      
     
     * 
     * These first are for the event handlers of connection and user added or removed 
     */
    void UserAddedEvent(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("User Added");
        engineUserID = (int)e.userId;
    }

    void UserRemovedEvent(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("User Removed");
    }

    void EmotivConnected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Connected!!");
    }

    void EmotivDisconnected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Disconnected");
    }

    /*
    * These are the functions themselves that handle the connections, the message_box.text are text fields in the GUI where the user writes down his EMOTIV credentials
    */ 
     
	public bool CloudConnected()
	{
		if (EmotivCloudClient.EC_Connect () == EdkDll.EDK_OK) {
			textOutputFunction("Status: Connection to server OK");
            Debug.Log("Status: Connection to server OK");
			if (EmotivCloudClient.EC_Login (userName.text, password.text)== EdkDll.EDK_OK) {
                textOutputFunction("Status: Login as " + userName.text);
                Debug.Log("Status: Login as " + userName.text);
				if (EmotivCloudClient.EC_GetUserDetail (ref userCloudID) == EdkDll.EDK_OK) {
                    textOutputFunction("Status: CloudID: " + userCloudID);
                    Debug.Log("Status: CloudID: " + userCloudID);
                    return true;
				}
			} 
			else 
			{
                textOutputFunction("Status: Cant login as " + userName.text+", check password is correct");
                Debug.Log("Status: Cant login as " + userName.text + ", check password is correct");
            }
		} 
		else 
		{
            textOutputFunction("Status: Cant connect to server");
            Debug.Log("Status: Cant connect to server");
        }
		return false;
	}

	public void SaveProfile(){
		if (CloudConnected ()) {
            int profileId = -1;
			profileId = EmotivCloudClient.EC_GetProfileId (userCloudID, profileName.text, ref profileId);
			if (profileId >= 0) {
				if (EmotivCloudClient.EC_UpdateUserProfile (userCloudID, (int)engineUserID, profileId) == EdkDll.EDK_OK) {
                    textOutputFunction("Status: Profile updated");
                    Debug.Log("Status: Profile updated");
                } else {
                    textOutputFunction("Status: Error saving profile, aborting");
                    Debug.Log("Status: Error saving profile, aborting");
                }
			} else {
				if (EmotivCloudClient.EC_SaveUserProfile (
					userCloudID, engineUserID, profileName.text, 
					EmotivCloudClient.profileFileType.TRAINING) == EdkDll.EDK_OK) {
                    textOutputFunction("Status: Profiled saved successfully");
                    Debug.Log("Status: Profiled saved successfully");
                } else {
                    textOutputFunction("Status: Error saving profile, aborting");
                    Debug.Log("Status: Error saving profile, aborting");
                }
			}
		}

	}

	public void LoadProfile(){
		if (CloudConnected ()) {
            int profileId = -1;
            if (EmotivCloudClient.EC_LoadUserProfile(
                userCloudID, (int)engineUserID,                
				EmotivCloudClient.EC_GetProfileId(userCloudID, profileName.text, ref profileId), 
				(int)version) == EdkDll.EDK_OK) {
                textOutputFunction("Status: Load finished");
                Debug.Log("Status: Load finished");
            } 
			else {
                textOutputFunction("Status: Problem loading");
                Debug.Log("Status: Problem loading");
            }
		}
	}

    /*
    * End of connection functions
    */

    #endregion

    #region Offline Store data

    public void CheckUserStorageDataPaths()
    {
        if (userSaveDataPath.text.Equals(""))
        {
            profileNameForSavingUser = "EMOTIVDataLog\\DataUserID" + currentPersonId.ToString() + ".emu";
            userSaveDataPath.text = profileNameForSavingUser;
        }
        else
        {
            profileNameForSavingUser = userSaveDataPath.text;
        }

        if (userLoadDataPath.text.Equals(""))
        {
            profileNameForLoadingUser = "EMOTIVDataLog\\DataUserID" + currentPersonId.ToString() + ".emu";
            userLoadDataPath.text = profileNameForLoadingUser;
        }
        else
        {
            profileNameForLoadingUser = userLoadDataPath.text;
        }

        if (userOfflineID.text.Equals(""))
        {
            userOfflineStoredID = (uint)currentPersonId;
        }
        else
        {
            if (!uint.TryParse(userOfflineID.text, out userOfflineStoredID))
            {
                Debug.Log("Error reading ID number");
                statusOfflineText.text = "Error reading ID number, please check numer is an integer";
            }
        }

    }

    public void LoadProfileOffline()
    {
        CheckUserStorageDataPaths();
        if (EdkDll.IEE_LoadUserProfile(userOfflineStoredID, profileNameForLoadingUser) == EdkDll.EDK_OK)
        {
            Debug.Log("Profile successfuly loaded.");
            statusOfflineText.text = "Profile successfuly loaded.";
        }
        else
        {
            Debug.Log("Error loading profile");
            statusOfflineText.text = "Error loading profile";
        }
    }

    public void SaveProfileOffline()
    {
        CheckUserStorageDataPaths();
        if (EdkDll.IEE_SaveUserProfile(userOfflineStoredID, profileNameForSavingUser) == EdkDll.EDK_OK)
        {
            Debug.Log("Profile successfuly saved.");
            statusOfflineText.text = "Profile successfuly saved.";
        }
        else
        {
            Debug.Log("Error saving profile");
            statusOfflineText.text = "Error saving profile";
        }
    }

    #endregion

}
