using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToScrolldownTrigger : MonoBehaviour {
    public ScrolldownContent scrollViewBody;
    public string windowName, scrollDownText;

	public void LaunchScrolldownWindow()
    {
        scrollViewBody.LaunchScrollDown(windowName, scrollDownText);
    }
}
