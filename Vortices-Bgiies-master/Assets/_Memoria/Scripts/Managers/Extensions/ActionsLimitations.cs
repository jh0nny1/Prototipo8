using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsLimitations : MonoBehaviour {

    public List<Action> limitedActionsArrayList = new List<Action>();

    public bool AddLimitationToAction(bool limitation, int actionIndex)
    {
        //¿Why is there a new arraylist? because it's the only way to use an "auxiliar" variable that will not be deleted or changed, otherwise as
        //  the operator works, the asociation would be with the Action variable and not the anonym. function itself.
        // This should not use many resources as it only holds references.
        limitedActionsArrayList.Add( ActionManager.Instance.currentActionList[actionIndex]);
        ActionManager.Instance.currentActionList[actionIndex] = () => ActionManager.Instance.ActionPairing(
            limitation,
            limitedActionsArrayList[limitedActionsArrayList.Count - 1]);

        return true;
    }
    
}
