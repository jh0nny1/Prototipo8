using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrolldownButtons : MonoBehaviour {

    public GameObject buttonPrefab;
    public GameObject optionButtonList, configOptionsTop;
    public Text windowTopNameText;
    List<GameObject> buttonList = new List<GameObject>();

    public GameObject AddButtonToList(Action functionToTrigger, string buttonText)
    {
        GameObject button = Instantiate(buttonPrefab);
        button.transform.SetParent(optionButtonList.transform);//Setting button parent
        button.GetComponent<Button>().onClick.AddListener(() => functionToTrigger());//Setting what button does when clicked
                                                                   //Next line assumes button has child with text as first gameobject like button created from GameObject->UI->Button
        button.transform.GetChild(0).GetComponent<Text>().text = buttonText;//Changing text
        buttonList.Add(button);
        return button;
    }

    public void ShowButtonList(string windowTopName)
    {
        windowTopNameText.text = windowTopName;
        configOptionsTop.SetActive(true);
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        ClearButtonList();
        buttonList = new List<GameObject>();
    }

    void ClearButtonList()
    {
        foreach(GameObject button in buttonList)
        {
            Destroy(button);
            Debug.Log("button destroyed");
        }
    }

}
