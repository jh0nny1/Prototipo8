using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrainingMenu : MonoBehaviour {

    public GameObject lightNeutral;
    public GameObject lightMarcar;
    public GameObject lightAtras;
    public GameObject lightElegir;
    public GameObject lightPlus;
    public GameObject lightMinus;
    public GameObject[] rotatingFigures;
    public GameObject rotatingFigureCamera;
    public ScrolldownContent statusViewScrolldown;

    EmotivCtrl emotivController;
    Vector3 rotatingFigureOriginalPosition;
    int activeRotatingFigure;
    bool loopFigureInitiated = false;
    public Dropdown rotatingFiguresDropdown, pickTrainingDropdown;

    string inputName = "EmotivInsight";
    
    private Emotiv.EdkDll.IEE_MentalCommandAction_t[] mentalCommands = new Emotiv.EdkDll.IEE_MentalCommandAction_t[6];
    private bool addedPush = false, addedPull = false, addedLift = false, addedDrop = false, addedLeft = false;
    

    /*
     * It will add 
     */
    void OnEnable()
    {
        emotivController = InterfaceManager.Instance.eegManager.emotivControl;
        AddTrainingControlEmotivConfig();
        Debug.Log("emotiv menu enabled");
        mentalCommands[0] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL;
        mentalCommands[1] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH;
        mentalCommands[2] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL;
        mentalCommands[3] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT;
        mentalCommands[4] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP;
        mentalCommands[5] = Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT;
        //ActivateEmotivCommands();
        rotatingFigureOriginalPosition = rotatingFigures[0].transform.position;
        emotivController.textOutputFunction = statusViewScrolldown.AddToScrolldown;
        InterfaceManager.Instance.eegManager.StartEmotivInsight();
    }

    void OnDisable()
    {
        RemoveVorticesControlEmotivConfig();
        Debug.Log("emotiv menu disabled");
    }


    private void AddTrainingEmotivLights(GameObject light, Emotiv.EdkDll.IEE_MentalCommandAction_t command)
    {
        ActionManager.Instance.updateActionsVorticesEmotivConfig.Add(() =>
               ActionManager.Instance.ActionPairing(
                   ActionManager.Instance.ActionConditionEmotiv(command),
                   light.GetComponent<ChangeColor>().ChangeGreen,
                   light.GetComponent<ChangeColor>().ChangeRed));
    }

    private void AddTrainingEmotivObjectMovement(Action movement, Emotiv.EdkDll.IEE_MentalCommandAction_t command)
    {
        ActionManager.Instance.updateActionsVorticesEmotivConfig.Add(() =>
               ActionManager.Instance.ActionPairing(
                   ActionManager.Instance.ActionConditionEmotiv(command),
                   movement));
    }

    private void AddTrainingControlEmotivConfig()
    {
        AddTrainingEmotivLights(lightNeutral, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL);
        AddTrainingEmotivLights(lightMarcar, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        AddTrainingEmotivLights(lightAtras, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        AddTrainingEmotivLights(lightElegir, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT);
        AddTrainingEmotivLights(lightPlus, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP);
        AddTrainingEmotivLights(lightMinus, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT);

        AddTrainingEmotivObjectMovement(FigurePush, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        AddTrainingEmotivObjectMovement(FigurePull, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        AddTrainingEmotivObjectMovement(FigureUp, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT);
        AddTrainingEmotivObjectMovement(FigureDown, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP);
        AddTrainingEmotivObjectMovement(FigureLeft, Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT);

    }

    public void SetActiveTrainingCommand()
    {
        switch (pickTrainingDropdown.value)
        {
            case 0:
                AddPushTraining();
                break;
            case 1:
                AddPullTraining();
                break;
            case 2:
                AddLiftTraining();
                break;
            case 3:
                AddDropTraining();
                break;
            case 4:
                AddLeftTraining();
                break;
            case 5:
                AddNeutralTraining();
                break;
            case 6:
                SetNoTraining();
                break;

        }
    }

    void AddNeutralTraining()
    {
        emotivController.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL);
        MOTIONSManager.Instance.AddLines(inputName, "Training set for Neutral", "");
        statusViewScrolldown.AddToScrolldown("Training set for Neutral");
    }

    void SetNoTraining()
    {
        emotivController.NoneTrainingControlCommand();
        MOTIONSManager.Instance.AddLines(inputName, "Training set for None", "");
        statusViewScrolldown.AddToScrolldown("Training set for None");
    }

    void AddPushTraining()
    {
        if (!addedPush)
        {
            addedPush = true;
            emotivController.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        }
        emotivController.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
        MOTIONSManager.Instance.AddLines(inputName, "Training set for Push", "");
        statusViewScrolldown.AddToScrolldown("Training set for Push");
    }

    void AddPullTraining()
    {       if (!addedPull)
        {
            addedPull = true;
            emotivController.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        }
        emotivController.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        MOTIONSManager.Instance.AddLines(inputName, "Training set for Pull", "");
        statusViewScrolldown.AddToScrolldown("Training set for Pull");
    }

    void AddLiftTraining()
    {
        if (!addedLift)
        {
            addedLift = true;
            emotivController.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT);
        }
        emotivController.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT);
        MOTIONSManager.Instance.AddLines(inputName,"Training set for Lift", "");
        statusViewScrolldown.AddToScrolldown("Training set for Lift");

    }

    void AddDropTraining()
    {
        if (!addedDrop)
        {
            addedDrop = true;
            emotivController.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP);
        }
        emotivController.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP);
        MOTIONSManager.Instance.AddLines(inputName, "Training set for Drop", "");
        statusViewScrolldown.AddToScrolldown("Training set for Drop");
    }

    void AddLeftTraining()
    {
        if (!addedLeft)
        {
            addedLeft = true;
            emotivController.AddActiveCommand(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT);
        }
        emotivController.SetTraining(Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT);
        MOTIONSManager.Instance.AddLines(inputName, "Training set for Left", "");
        statusViewScrolldown.AddToScrolldown("Training set for Left");
    }

    private IEnumerator ResetTrainingCoroutine()
    {
        //resets all the training
        for (int i = 0;  i< 6; i++)
        {
            emotivController.ResetTraining(mentalCommands[i]);
            yield return null;
        }
    }

    public void ResetTraining()
    {
        StartCoroutine("ResetTrainingCoroutine");
    }

    public void StartTraining()
    {
        emotivController.StartTraining();
    }

    public void EraseTraining()
    {
        emotivController.EraseTraining();
    }

    public void RejectTraining()
    {
        emotivController.RejectTraining();
    }

    void RemoveVorticesControlEmotivConfig()
    {
        ActionManager.Instance.updateActionsVorticesEmotivConfig.Clear();
    }

    public void ChangeRotatingFigure()
    {
        rotatingFigures[activeRotatingFigure].SetActive(false);
        activeRotatingFigure = rotatingFiguresDropdown.value;
        rotatingFigures[activeRotatingFigure].SetActive(true);
    }

    public void FigurePush()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.forward * 10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigurePull()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.forward * -10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigureLeft()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.right * -10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigureRight()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.right * 10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigureUp()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.up * 10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    public void FigureDown()
    {
        if (loopFigureInitiated)
            return;
        loopFigureInitiated = true;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = false;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().AddForce(rotatingFigureCamera.GetComponent<Camera>().transform.up * -10, ForceMode.Impulse);
        StartCoroutine("SetTimerResetFigure", 2.3f);
        
    }

    private IEnumerator SetTimerResetFigure(float time)
    {
        float startTime = Time.unscaledTime;
        while ((Time.unscaledTime - startTime)<time)
        {
            yield return null;
        }
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].GetComponent<Rigidbody>().velocity = Vector3.zero;
        rotatingFigures[activeRotatingFigure].transform.position = rotatingFigureOriginalPosition;
        rotatingFigures[activeRotatingFigure].GetComponentInParent<Animation>().enabled = true;
        loopFigureInitiated = false;
        
    }

    
}
