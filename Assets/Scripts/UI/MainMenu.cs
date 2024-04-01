using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Defines behaviour for PlayButton
    public void PlayGame()
    {
        SceneManager.LoadScene("Saves");
    }

    // Defines behaviour for SettingsButton
    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    // Defines behaviour for CreditsButton
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    // Defines behaviour for ExitButton
    public void QuitGame()
    {
        Debug.Log("Quit!");
        // Doesn't do anything in Unity, only works in actual games
        Application.Quit();
    }
}