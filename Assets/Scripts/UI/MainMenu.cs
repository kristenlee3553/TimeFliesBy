using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Change later to load save slots instead
        SceneManager.LoadScene("GameScene");
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        // Doesn't do anything in Unity, only works in actual games
        Application.Quit();
    }
}