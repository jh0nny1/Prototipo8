using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrolldownContent : MonoBehaviour {
    public Text scrolldownContentText, scrolldownContentName;
    public GameObject scrolldownTopBar;

    public void LaunchScrollDown(string windowName, string windowText)
    {
        scrolldownContentName.text = windowName;
        scrolldownContentText.text = windowText;
        if(scrolldownTopBar != null)
            scrolldownTopBar.SetActive(true);
        gameObject.SetActive(true);
    }

    public void AddToScrolldown(string text)
    {
        scrolldownContentText.text = scrolldownContentText.text + "\n" + text;
    }

}
