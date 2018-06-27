using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour {

    private void Start()
    {
        try
        {
            if (MOTIONSManager.Instance.instanced)
            {
                Destroy(gameObject);
            }
        }
        catch
        {
            SceneManager.LoadScene("ConfigPrototype");
        }        
    }
}
