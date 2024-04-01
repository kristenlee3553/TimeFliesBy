using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("HomePage");
        }
    }
    
    // Defines behaviour for BackButton
    public void BackButton()
    {
        SceneManager.LoadScene("HomePage");
    }
}