using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    // Defines behaviour for tutorial button
    public void PlayTutorial()
    {
        GameManager.s_level = "PreTut";
        SceneManager.LoadScene("GameScene");
    }

    // Defines behaviour for level 1 button
    public void PlayLevel1()
    {
        GameManager.s_level = "Dino";
        SceneManager.LoadScene("GameScene");
    }

    // Defines behaviour for level 2 button
    public void PlayLevel2()
    {
        GameManager.s_level = "MedOne";
        SceneManager.LoadScene("GameScene");
    }

    // Defines behaviour for back button
    public void BackButton()
    {
        SceneManager.LoadScene("Saves");
    }
}
