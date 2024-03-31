using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlots : MonoBehaviour
{
    public void LoadSaves()
    {
        SceneManager.LoadScene("GameScene");
    }
}
